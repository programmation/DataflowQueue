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
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Console.WriteLine ("Application Started!");

			global::Xamarin.Forms.Forms.Init ();

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
//			Xamarin.Calabash.Start ();
			#endif

			var builder = new ContainerBuilder ();

			builder.RegisterGeneric (typeof(TouchConcurrentQueue<>)).As (typeof(IPclConcurrentQueue<>));
			builder.RegisterGeneric (typeof(TouchBlockingCollection<>)).As (typeof(IPclBlockingCollection<>));
			builder.RegisterType<NativeLogger> ().As<INativeLogger> ().SingleInstance ();

			var readerParallelism = 2;

			var stringLoader = new TransformBlock<string, WordFinderString> (
				new Func<string, Task<WordFinderString>> (async (path) => {
					var logger = IoC.Container.Resolve<ILogger> ();

					logger.Debug (this, "Loading {0}", (object)path);

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

					return new WordFinderString(path, optionalString);
				}),
				new ExecutionDataflowBlockOptions { MaxMessagesPerTask = readerParallelism, }
			);
				
			var reversedWordFinder = new TransformManyBlock<WordFinderArray, WordFinderString> (input => {
				var title = Path.GetFileName (input.Uri);
				var optionalWords = input.Result;

				var logger = IoC.Container.Resolve<ILogger> ();
				var reversibleWords = new ConcurrentQueue<WordFinderString> ();

				if (optionalWords.IsFaulted) {
					reversibleWords.Enqueue(new WordFinderString(input.Uri, new Optional<string>(optionalWords.Fault)));
				} else {
					logger.Debug(this, "{0}: Checking for reversible words...", (object)title);

					var words = optionalWords.Value;

					// Parallel not available in PCL on Mono
					Parallel.ForEach (words, word => {
						var reverse = new string(word.ToCharArray ().Reverse ().ToArray ());
						if (Array.BinarySearch<string> (words, reverse) >= 0 && word != reverse) {
							reversibleWords.Enqueue (new WordFinderString(input.Uri, new Optional<string>(word)));
						}
					});
				}

				return reversibleWords;
			});

			LoadApplication (new App (builder, stringLoader, reversedWordFinder));

			return base.FinishedLaunching (app, options);
		}
	}
}

