using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Autofac;

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

			builder.RegisterGeneric (typeof(DroidConcurrentQueue<>)).As (typeof(IPclConcurrentQueue<>));

			LoadApplication (new App (builder));
		}
	}
}

