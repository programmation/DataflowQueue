using System;
using System.Threading.Tasks.Dataflow;
//using System.Threading.Tasks.Parallel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using DataflowQueue;
using Services;
using Xamarin.Forms;
using OptionalString = DataflowQueue.Optional<string>;
using OptionalStringArray = DataflowQueue.Optional<string[]>;
using WordFinderString = DataflowQueue.WordFinderResult<string>;
using WordFinderArray = DataflowQueue.WordFinderResult<string[]>;
using System.IO;
using System.ComponentModel;

namespace DataflowQueue
{
	public class WordFinderResult<T>
	{
		public string Uri { get; private set; }
		public Optional<T> Result { get; private set; }

		public WordFinderResult(string uri, Optional<T> result)
		{
			Uri = uri;
			Result = result;
		}
	}

	public class ReversedWordFinder
	{
		private ILogger _logger;
		private INativeReversedWordFinder _nativeReversedWordFinder;
		private TransformBlock<string, WordFinderString> _loadStringAsync;
		private TransformBlock<string, WordFinderString> _downloadStringAsync;
		private TransformBlock<WordFinderString, WordFinderArray> _createWordList;
		private TransformBlock<WordFinderArray, WordFinderArray> _filterWordList;
		private TransformManyBlock<WordFinderArray, WordFinderString> _findReversedWords;
		private ActionBlock<WordFinderString> _printResults;

		public event Action<string, string> ProgressReporter;

		public ReversedWordFinder ()
		{
			_logger = IoC.Container.Resolve<ILogger> ();
			_nativeReversedWordFinder = IoC.Container.Resolve<INativeReversedWordFinder> ();

			var readerParallelism = DataflowBlockOptions.Unbounded;
			var analyserParallelism = DataflowBlockOptions.Unbounded;
			var printerParallelism = DataflowBlockOptions.Unbounded;

			ProgressReporter += Report; // ensure that ProgressReporter != null

			_loadStringAsync = _nativeReversedWordFinder.AsyncStringLoader;

			_downloadStringAsync = new TransformBlock<string, WordFinderString> (
				new Func<string, Task<WordFinderString>> (async uri => {
					return await DoDownloadAsync (uri);
				}),
				new ExecutionDataflowBlockOptions { MaxMessagesPerTask = readerParallelism, }
			);

			_createWordList = new TransformBlock<WordFinderString, WordFinderArray> (input => {
				return DoCreateWordList (input);
			}, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = analyserParallelism });

			_filterWordList = new TransformBlock<WordFinderArray, WordFinderArray> (input => {
				return DoFilterWordList (input);
			}, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = analyserParallelism });

			if (_nativeReversedWordFinder.ReversedWordFinder != null) {
				_findReversedWords = _nativeReversedWordFinder.ReversedWordFinder;
				_nativeReversedWordFinder.ProgressReporter += NativeReport;
			} else {
				_findReversedWords = new TransformManyBlock<WordFinderArray, WordFinderString> (input => {
					return DoFindReversedWords (input);
				});
			}

			_printResults = new ActionBlock<WordFinderString> (input => {
				DoPrint (input);
			}, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = printerParallelism });

			_loadStringAsync.LinkTo (_createWordList, new DataflowLinkOptions { PropagateCompletion = true, });
			_downloadStringAsync.LinkTo (_createWordList, new DataflowLinkOptions { PropagateCompletion = true, });
			_createWordList.LinkTo (_filterWordList, new DataflowLinkOptions { PropagateCompletion = true, });
			_filterWordList.LinkTo (_findReversedWords, new DataflowLinkOptions { PropagateCompletion = true, });
			_findReversedWords.LinkTo (_printResults, new DataflowLinkOptions { PropagateCompletion = true, });
		}

		private async Task<WordFinderString> DoDownloadAsync (string uri)
		{
			var title = Path.GetFileName (uri);

			ProgressReporter (title, "Downloading...");
			_logger.Debug (this, "{0}: Downloading {1}", (object)title, (object)uri);	

			Optional<string> optionalString = null;
			try {
				var result = await new HttpClient ().GetStringAsync (uri);
				ProgressReporter (title, "Done!");
				optionalString = new Optional<string> (result);
			} catch (Exception ex) {
				ProgressReporter (title, String.Format ("Failed! {0}", ex));
				optionalString = new Optional<string> (ex);
			}

			return new WordFinderString (uri, optionalString);
		}

		private WordFinderArray DoCreateWordList(WordFinderString input)
		{
			var title = Path.GetFileName (input.Uri);
			var optionalText = input.Result;

			if (optionalText.IsFaulted) {
				ProgressReporter (title, String.Format ("Failed! {0}", optionalText.Fault));
				return new WordFinderArray(input.Uri, new Optional<string[]> (optionalText.Fault));
			}

			ProgressReporter (title, "Creating word list...");
			_logger.Debug (this, "{0}: Creating word list...", (object)title);

			var text = optionalText.Value;

			char[] tokens = text.ToCharArray ();
			for (int i = 0; i < tokens.Length; i++) {
				if (!char.IsLetter (tokens [i])) {
					tokens [i] = ' ';
				}
			}
			text = new string (tokens);

			var words = text.Split (new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			ProgressReporter (title, String.Format ("Found {0} words", words.Length));
			_logger.Debug (this, "{1}: Found {0} words", words.Length, (object)title);

			return new WordFinderArray (input.Uri, new Optional<string[]> (words));
		}

		private WordFinderArray DoFilterWordList(WordFinderArray input)
		{
			var title = Path.GetFileName (input.Uri);
			var optionalWords = input.Result;

			if (optionalWords.IsFaulted) {
				ProgressReporter (title, String.Format ("Failed! {0}", optionalWords.Fault));
				return new WordFinderArray(input.Uri, new Optional<string[]> (optionalWords.Fault));
			}	

			ProgressReporter (title, String.Format ("Filtering word list..."));
			_logger.Debug (this, "{0}: Filtering word list...", (object)title);

			var words = optionalWords.Value
				.Where (word => word.Length > 2)
				.OrderBy (word => word.ToLowerInvariant ())
				.Distinct ()
				.ToArray ();

			ProgressReporter (title, String.Format ("Filtered {0} words", words.Length));
			_logger.Debug (this, "{1}: Filtered {0} words", words.Length, (object)title);

			return new WordFinderArray (input.Uri, new Optional<string[]> (words));
		}

		private IEnumerable<WordFinderString> DoFindReversedWords(WordFinderArray input)
		{
			var title = Path.GetFileName (input.Uri);
			var optionalWords = input.Result;

			var reversedWords = IoC.Container.Resolve<IPclConcurrentQueue<WordFinderString>> ();

			if (optionalWords.IsFaulted) {
				ProgressReporter (title, String.Format ("Failed! {0}", optionalWords.Fault));
				reversedWords.Enqueue (new WordFinderString(input.Uri, new Optional<string> (optionalWords.Fault)));
			} else {
				ProgressReporter (title, String.Format ("Checking for reversible words..."));
				_logger.Debug (this, "{0}: Checking for reversible words...", (object)title);

				var words = optionalWords.Value;

				foreach (var word in words) {
					var reverse = new string (word.ToCharArray ().Reverse ().ToArray ());
					if (Array.BinarySearch<string> (words, reverse) >= 0 && word != reverse) {
						reversedWords.Enqueue (new WordFinderString(input.Uri, new Optional<string> (word)));
					}
				}
				ProgressReporter (title, String.Format ("Found {0} reversible words!", reversedWords.Count));
				_logger.Debug (this, "{0}: Found {1} reversible words!", (object)title, (object)reversedWords.Count);
			}

			return (IEnumerable<WordFinderString>)reversedWords;
		}

		private void DoPrint(WordFinderString input)
		{
			var title = Path.GetFileName (input.Uri);
			var optionalWord = input.Result;

			if (optionalWord.IsFaulted) {
				ProgressReporter (title, String.Format ("Failed! {0}", optionalWord.Fault));
				_logger.Debug (this, "Failed! {0}", optionalWord.Fault);
				return;
			}	

			var reversedWord = optionalWord.Value;
			var word = new string (reversedWord.ToCharArray ().Reverse ().ToArray ());

			ProgressReporter (title, String.Format ("{0} / {1}", word, reversedWord));

			// Have to cast params to object otherwise compiler hooks the call up to the wrong logging method
			_logger.Debug (this, "{2}: Found reversed word {0} / {1}", (object)reversedWord, (object)word, (object)title);
		}

		public void Post (string uri)
		{
			if (Uri.IsWellFormedUriString (uri, UriKind.Absolute)) {
				var url = new Uri (uri);
				if (url.Scheme.ToLower () == "http") {
					_downloadStringAsync.Post (uri);
					return;
				}
			}
			_loadStringAsync.Post (uri);
		}

		public async Task SendAsync(string uri)
		{
			if (Uri.IsWellFormedUriString (uri, UriKind.Absolute)) {
				var url = new Uri (uri);
				if (url.Scheme.ToLower () == "http") {
					await _downloadStringAsync.SendAsync (uri);
					return;
				}
			}
			await _loadStringAsync.SendAsync (uri);
		}

		public void Complete()
		{
			_downloadStringAsync.Complete ();
			_loadStringAsync.Complete ();
			_printResults.Completion.Wait ();
		}

		private void Report(string s1, string s2)
		{			
		}

		private void NativeReport(string title, string message)
		{
			ProgressReporter (title, message);
		}
	}
}

