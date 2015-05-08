using System;
using Xamarin.Forms;
using System.Windows.Input;
using Autofac;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataflowQueue
{
	public class App : Application
	{
		Label label1;
		Button button1;
		Label label2;
		TransformManyBlock<Optional<string[]>, Optional<string>> _reversedWordFinder;

		public App (ContainerBuilder builder, TransformManyBlock<Optional<string[]>, Optional<string>> reversedWordFinder = null)
		{
			_reversedWordFinder = reversedWordFinder;
			builder.Build ();

			label1 = new Label {
				XAlign = TextAlignment.Center,
				Text = "Welcome!",
			};
			button1 = new Button {
				Text = "Go!",
				Command = GoCommand,
			};
			label2 = new Label {
				XAlign = TextAlignment.Start,
			};
				
			// The root page of your application
			MainPage = new NavigationPage (
				new ContentPage {
					Content = new StackLayout {
						VerticalOptions = LayoutOptions.Start,
						Children = {
							label1,
							button1,
							label2,
						}
					}
				}
			);
		}

		public ICommand GoCommand {
			get {
				return new Command (() => {
					DoGo ();
				});
			}
		}

		private bool _going = false;

		private void DoGo ()
		{
			if (_going)
				return;
			
			_going = true;

			var worker = new ReversedWordFinder (_reversedWordFinder);

			if (true) {
				worker.Post ("http://www.gutenberg.org/files/6130/6130-0.txt");
				worker.Post ("http://www.gutenberg.org/cache/epub/1727/pg1727.txt");
				worker.Post ("http://www.gutenberg.org/cache/epub/1635/pg1635.txt");
				_going = false;
			} else {
				Task.Run (async () => {
					await worker.SendAsync ("http://www.gutenberg.org/files/6130/6130-0.txt");
					_going = false;
				});
			}
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

