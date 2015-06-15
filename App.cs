using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Input;
using Autofac;
using DataflowQueue.Pages;
using Services;
using Xamarin.Forms;
using WordFinderArray = DataflowQueue.WordFinderResult<string[]>;
using WordFinderString = DataflowQueue.WordFinderResult<string>;

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

