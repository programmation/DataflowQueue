using System;
using System.Collections.Concurrent;
using Helpers;

namespace DataflowQueue.Droid
{
	public class NativeConcurrentQueue<T>
		: ConcurrentQueue<T>, IPclConcurrentQueue<T>
		where T : class
	{
		public NativeConcurrentQueue ()
		{
		}
	}
}

