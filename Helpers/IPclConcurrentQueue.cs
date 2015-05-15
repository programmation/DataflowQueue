using System;

namespace Helpers
{
	public interface IPclConcurrentQueue<T>
	{
		bool TryAdd (T item);

		bool TryTake (out T item);
	}

}
