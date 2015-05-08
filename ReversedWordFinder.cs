using System;
using System.Threading.Tasks.Dataflow;

//using System.Threading.Tasks.Parallel;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Linq;
using Xamarin.Forms;
using DataflowQueue;
using Autofac;
using OptionalString = DataflowQueue.Optional<string>;
using OptionalStringArray = DataflowQueue.Optional<string[]>;
using System.Collections;
using System.Collections.Generic;

namespace DataflowQueue
{
	public class ReversedWordFinder
	{
		TransformBlock<string, Optional<string>> _downloadStringAsync;
		TransformBlock<Optional<string>, Optional<string[]>> _createWordList;
		TransformBlock<Optional<string[]>, Optional<string[]>> _filterWordList;
		TransformManyBlock<Optional<string[]>, Optional<string>> _findReversedWords;
		ActionBlock<Optional<string>> _printResults;

		public ReversedWordFinder (TransformManyBlock<Optional<string[]>, Optional<string>> reversedWordFinder)
		{
			_downloadStringAsync = new TransformBlock<string, Optional<string>> (
				new Func<string, Task<Optional<string>>> (async uri => {
					return await DoDownloadAsync (uri);
				}),
				new ExecutionDataflowBlockOptions { MaxMessagesPerTask = DataflowBlockOptions.Unbounded, }
			);

			_createWordList = new TransformBlock<Optional<string>, Optional<string[]>> (optionalText => {
				return DoCreateWordList (optionalText);
			});

			_filterWordList = new TransformBlock<Optional<string[]>, Optional<string[]>> (optionalWords => {
				return DoFilterWordList (optionalWords);
			});

			if (reversedWordFinder != null) {
				_findReversedWords = reversedWordFinder;
			} else {
				_findReversedWords = new TransformManyBlock<Optional<string[]>, Optional<string>> (optionalWords => {
					return DoFindReversedWords (optionalWords);
				});
			}

			_printResults = new ActionBlock<Optional<string>> (optionalWord => {
				DoPrint (optionalWord);
			});

			_downloadStringAsync.LinkTo (_createWordList, new DataflowLinkOptions { PropagateCompletion = true, });
			_createWordList.LinkTo (_filterWordList, new DataflowLinkOptions { PropagateCompletion = true, });
			_filterWordList.LinkTo (_findReversedWords, new DataflowLinkOptions { PropagateCompletion = true, });
			_findReversedWords.LinkTo (_printResults, new DataflowLinkOptions { PropagateCompletion = true, });
		}

		private async Task<Optional<string>> DoDownloadAsync (string uri)
		{
			Debug.WriteLine ("Downloading {0}", uri);	

			Optional<string> optionalString = null;
			try {
				var result = await new HttpClient ().GetStringAsync (uri);
				optionalString = new Optional<string> (result);
			} catch (Exception ex) {
				optionalString = new Optional<string> (ex);
			}

			return optionalString;
		}

		private Optional<string[]> DoCreateWordList(Optional<string> optionalText)
		{
			if (optionalText.IsFaulted) {
				return new Optional<string[]> (optionalText.Fault);
			}

			Debug.WriteLine ("Creating word list...");

			var text = optionalText.Value;

			char[] tokens = text.ToCharArray ();
			for (int i = 0; i < tokens.Length; i++) {
				if (!char.IsLetter (tokens [i])) {
					tokens [i] = ' ';
				}
			}
			text = new string (tokens);

			var words = text.Split (new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			return new Optional<string[]> (words);
		}

		private Optional<string[]> DoFilterWordList(Optional<string[]> optionalWords)
		{
			if (optionalWords.IsFaulted) {
				return new Optional<string[]> (optionalWords.Fault);
			}	

			Debug.WriteLine ("Filtering word list...");

			var wordList = optionalWords.Value
				.Where (word => word.Length > 2)
				.OrderBy (word => word.ToLowerInvariant ())
				.Distinct ()
				.ToArray ();

			Debug.WriteLine ("Found {0} words", wordList.Length);

			return new Optional<string[]> (wordList);
		}

		private IEnumerable<Optional<string>> DoFindReversedWords(Optional<string[]> optionalWords)
		{
			var reversedWords = IoC.Container.Resolve<IPclConcurrentQueue<OptionalString>> ();

			if (optionalWords.IsFaulted) {
				reversedWords.Enqueue (new Optional<string> (optionalWords.Fault));
			} else {
				Debug.WriteLine ("Checking for reversible words...");

				var words = optionalWords.Value;

				foreach (var word in words) {
					var reverse = new string (word.ToCharArray ().Reverse ().ToArray ());

					if (Array.BinarySearch<string> (words, reverse) >= 0 && word != reverse) {
						reversedWords.Enqueue (new Optional<string> (word));
					}
				}
			}
			return (IEnumerable<Optional<string>>)reversedWords;
		}

		private void DoPrint(Optional<string> optionalWord)
		{
			if (optionalWord.IsFaulted) {
				Debug.WriteLine ("Failed! {0}", optionalWord.Fault);
				return;
			}	

			var reversedWord = optionalWord.Value;

			Debug.WriteLine ("Found reversed word {0} / {1}", reversedWord, new string (reversedWord.ToCharArray ().Reverse ().ToArray ()));
		}

		public void Post (string uri)
		{
			_downloadStringAsync.Post (uri);
		}

		public async Task SendAsync(string uri)
		{
			await _downloadStringAsync.SendAsync (uri);
		}

		public void Complete()
		{
			_downloadStringAsync.Complete ();
			_printResults.Completion.Wait ();
		}

	}
}

