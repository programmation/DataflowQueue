using System;
using System.Collections.Concurrent;
using DataflowQueue.Droid;

namespace DataflowQueue.Droid
{
	public class DroidConcurrentQueue<T>
		: ConcurrentQueue<T>, IPclConcurrentQueue<T>
		where T : class
	{
	}

}

