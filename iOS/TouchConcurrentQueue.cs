using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Xamarin.Forms;
using DataflowQueue.iOS;


namespace DataflowQueue.iOS
{
	public class TouchConcurrentQueue<T>
		: ConcurrentQueue<T>, IPclConcurrentQueue<T>
		where T : class
	{
		public TouchConcurrentQueue ()
		{
		}
	}
}

