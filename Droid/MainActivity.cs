using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autofac;
using Dataflow.Droid.Services;
using Helpers;
using Services;

namespace DataflowQueue.Droid
{
	[Activity (Label = "DataflowQueue.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			var builder = new ContainerBuilder ();

			builder.RegisterGeneric (typeof(NativeConcurrentQueue<>)).As (typeof(IPclConcurrentQueue<>));
			builder.RegisterGeneric (typeof(NativeBlockingCollection<>)).As (typeof(IPclBlockingCollection<>));
			builder.RegisterType<NativeLogger> ().As<INativeLogger> ().SingleInstance ();
			builder.RegisterType<NativeReversedWordFinder> ().As<INativeReversedWordFinder> ();

			LoadApplication (new App (builder));
		}
	}
}

