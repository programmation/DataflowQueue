using System;
using Xamarin.Forms;

namespace DataflowQueue
{
	public interface IPclConcurrentQueue<T>
		where T : class
	{
		void Enqueue (T item);

	}

}

