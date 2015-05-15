using System.Collections.Concurrent;
using Helpers;

public class DroidBlockingCollection<T> 
	: BlockingCollection<T>, IPclBlockingCollection<T>
{
	//		public bool IsCompleted {
	//			get {
	//				return ((BlockingCollection<T>)this).IsCompleted;
	//			}
	//		}
	//
	//		public void Add (T item)
	//		{
	//			((BlockingCollection<T>)this).Add (item);
	//		}
	//
	//		public T Take ()
	//		{
	//			return ((BlockingCollection<T>)this).Take ();
	//		}
}