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

			var reversedWordFinder = new TransformManyBlock<Optional<string[]>, Optional<string>> (optionalWords => {
				var logger = IoC.Container.Resolve<ILogger> ();
				var reversedWords = new ConcurrentQueue<Optional<string>> ();

				if (optionalWords.IsFaulted) {
					reversedWords.Enqueue(new Optional<string>(optionalWords.Fault));
				} else {
					logger.Debug(this, "Checking for reversible words...");

					var words = optionalWords.Value;

					// Parallel not available in PCL on Mono
					Parallel.ForEach (words, word => {
						var reverse = new string(word.ToCharArray ().Reverse ().ToArray ());

						if (Array.BinarySearch<string> (words, reverse) >= 0 && word != reverse) {
							reversedWords.Enqueue (new Optional<string>(word));
						}
					});
				}

				return reversedWords;
			});

			LoadApplication (new App (builder, reversedWordFinder));

			return base.FinishedLaunching (app, options);
		}
	}
}

