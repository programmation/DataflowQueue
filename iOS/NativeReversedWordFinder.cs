using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Autofac;
using Dataflow.iOS.Services;
using Foundation;
using Helpers;
using Services;
using UIKit;
using WordFinderString = DataflowQueue.WordFinderResult<string>;
using WordFinderArray = DataflowQueue.WordFinderResult<string[]>;
using System.IO;
using System.Text;

namespace DataflowQueue.iOS
{

	public class NativeReversedWordFinder
		: INativeReversedWordFinder
	{
		private ILogger _logger;

		public TransformBlock<string, WordFinderString> AsyncStringLoader { get; private set; }
		public TransformManyBlock<WordFinderArray, WordFinderString> ReversedWordFinder { get; private set; }

		public event Action<string, string> ProgressReporter;

		public NativeReversedWordFinder ()
		{
			_logger = IoC.Container.Resolve<ILogger> ();

			ProgressReporter += Report;

			var readerParallelism = DataflowBlockOptions.Unbounded;
			var analyserParallelism = DataflowBlockOptions.Unbounded;

			AsyncStringLoader = new TransformBlock<string, WordFinderString> (
				new Func<string, Task<WordFinderString>> (async (path) => {
					var title = Path.GetFileName (path);

					ProgressReporter (title, "Loading...");
					_logger.Debug (this, "Loading {0}", (object)path);

					Optional<string> optionalString = null;

					try {
						var uniencoding = new UTF8Encoding();
						byte[] fileText;

						using (var sourceStream = File.Open (path, FileMode.Open)) {
							fileText = new byte[sourceStream.Length];
							await sourceStream.ReadAsync (fileText, 0, (int)sourceStream.Length);
						}

						optionalString = new Optional<string> (uniencoding.GetString (fileText));
					} catch (Exception ex) {
						optionalString = new Optional<string> (ex);
					}

					ProgressReporter (title, String.Format ("Loaded {0} characters", optionalString.Value.Length));
					_logger.Debug (this, "Loaded {0} characters", (object)optionalString.Value.Length);

					return new WordFinderString(path, optionalString);
				}),
				new ExecutionDataflowBlockOptions { MaxMessagesPerTask = readerParallelism, }
			);

			ReversedWordFinder = new TransformManyBlock<WordFinderArray, WordFinderString> (input => {
				var title = Path.GetFileName (input.Uri);
				var optionalWords = input.Result;

				var reversibleWords = new ConcurrentQueue<WordFinderString> ();

				if (optionalWords.IsFaulted) {
					ProgressReporter (title, String.Format ("Failed! {0}", optionalWords.Fault));
					reversibleWords.Enqueue(new WordFinderString(input.Uri, new Optional<string>(optionalWords.Fault)));
				} else {
					ProgressReporter (title, String.Format ("Checking for reversible words..."));
					_logger.Debug(this, "{0}: Checking for reversible words...", (object)title);

					var words = optionalWords.Value;

					// Parallel not available in PCL on Mono
					Parallel.ForEach (words, word => {
						var reverse = new string(word.ToCharArray ().Reverse ().ToArray ());
						if (Array.BinarySearch<string> (words, reverse) >= 0 && word != reverse) {
//							ProgressReporter (title, String.Format ("Found {0} / {1}", word, reverse));
//							_logger.Debug (this, "{0}: Found {1} / {2}", (object)title, (object)word, (object)reverse);
							reversibleWords.Enqueue (new WordFinderString(input.Uri, new Optional<string>(word)));
						}
					});

					ProgressReporter (title, String.Format ("Found {0} reversible words!", reversibleWords.Count));
					_logger.Debug (this, "{0}: Found {1} reversible words!", (object)title, (object)reversibleWords.Count);
				}

				return reversibleWords;
			}, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = analyserParallelism });
		}

		private void Report(string s1, string s2)
		{			
		}
	}
}
