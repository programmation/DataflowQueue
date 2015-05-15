using System;

namespace Helpers
{
	public interface IPclBlockingCollection<T>
	{
		bool IsCompleted { get; }

		void CompleteAdding ();

		T Take ();

		bool TryTake (out T item, int millisecondsTimeout);

		void Add (T item);
	}
}
