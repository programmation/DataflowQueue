using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks.Dataflow;
using WordFinderString = DataflowQueue.WordFinderResult<string>;
using WordFinderArray = DataflowQueue.WordFinderResult<string[]>;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;

namespace DataflowQueue
{
	public partial class ReversibleWordsPage : ContentPage
	{
		ReversedWordFinder _worker;

		public string Url0 { get; set; }
		public string Url1 { get; set; }
		public string Url2 { get; set; }

		public string Book0 { get; set; }
		public string Book1 { get; set; }
		public string Book2 { get; set; }

		public ObservableCollection<ProgressStatus> ProgressReports0 { get; set; }
		public ObservableCollection<ProgressStatus> ProgressReports1 { get; set; }
		public ObservableCollection<ProgressStatus> ProgressReports2 { get; set; }

		public ReversibleWordsPage ()
		{
			InitializeComponent ();

			_worker = new ReversedWordFinder ();

			if (true) {
				Url0 = "Iliad.txt";
				Url1 = "Odyssey.txt";
				Url2 = "Ion.txt";
			} else {
				Url0 = "http://www.gutenberg.org/cache/epub/1727/pg1727.txt";
				Url1 = "http://www.gutenberg.org/files/6130/6130-0.txt";
				Url2 = "http://www.gutenberg.org/cache/epub/1635/pg1635.txt";
			}

			Book0 = Path.GetFileName (Url0);
			Book1 = Path.GetFileName (Url1);
			Book2 = Path.GetFileName (Url2);

			ProgressReports0 = new ObservableCollection<ProgressStatus> ();
			ProgressReports1 = new ObservableCollection<ProgressStatus> ();
			ProgressReports2 = new ObservableCollection<ProgressStatus> ();

			_worker.ProgressReporter += (string title, string message) => {
				Device.BeginInvokeOnMainThread (() => {
					var item = new ProgressStatus (title, message);
					if (title == Book0) {
						ProgressReports0.Add (item);
//						list0.ScrollTo (item, ScrollToPosition.MakeVisible, false);
					}
					if (title == Book1) {
						ProgressReports1.Add (item);
//						list1.ScrollTo (item, ScrollToPosition.MakeVisible, false);
					}
					if (title == Book2) {
						ProgressReports2.Add (item);
//						list2.ScrollTo (item, ScrollToPosition.MakeVisible, false);
					}
				});
			};

			BindingContext = this;
		}

		public ICommand Go {
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

			ProgressReports0.Clear ();
			ProgressReports1.Clear ();
			ProgressReports2.Clear ();

			#pragma warning disable 0162
			if (false) {
				_worker.Post (Url0);
				_worker.Post (Url1);
				_worker.Post (Url2);
				_going = false;
			} else {
				Task.Run (async () => {
					await _worker.SendAsync (Url0);
					await _worker.SendAsync (Url1);
					await _worker.SendAsync (Url2);
					_going = false;
				});
			}
			#pragma warning restore 0162
		}

	}
}

