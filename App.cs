using System;
using Xamarin.Forms;
using System.Windows.Input;
using Autofac;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Services;
using WordFinderString = DataflowQueue.WordFinderResult<string>;
using WordFinderArray = DataflowQueue.WordFinderResult<string[]>;
using System.Collections.ObjectModel;

namespace DataflowQueue
{
	public class App : Application
	{
		public App (ContainerBuilder builder)
		{
			builder.RegisterType<Logger> ().As<ILogger> ().SingleInstance ();

			IoC.Container = builder.Build ();

			// The root page of your application
			var mainPage = new TabbedPage ();
			mainPage.Children.Add (
				new NavigationPage (
					new ReversibleWordsPage ()
				) {
					Title = "ActionBlocks"
				}
			);

			MainPage = mainPage;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

