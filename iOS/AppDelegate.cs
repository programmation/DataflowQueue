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

			builder.RegisterGeneric (typeof(NativeConcurrentQueue<>)).As (typeof(IPclConcurrentQueue<>));
			builder.RegisterGeneric (typeof(NativeBlockingCollection<>)).As (typeof(IPclBlockingCollection<>));
			builder.RegisterType<NativeLogger> ().As<INativeLogger> ().SingleInstance ();
			builder.RegisterType<NativeReversedWordFinder> ().As<INativeReversedWordFinder> ();

			LoadApplication (new App (builder));

			return base.FinishedLaunching (app, options);
		}
	}

}

