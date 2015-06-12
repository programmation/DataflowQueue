using System;
using Xamarin.Forms;

namespace DataflowQueue
{
	public interface IPclConcurrentQueue<T>
		where T : class
	{
		int Count { get; }

		void Enqueue (T item);

	}

}

