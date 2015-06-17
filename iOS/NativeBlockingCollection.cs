using System.Collections.Concurrent;
using Helpers;

public class NativeBlockingCollection<T> 
	: BlockingCollection<T>, IPclBlockingCollection<T>
{
}