using System;
using System.Threading.Tasks;

namespace DataflowQueue
{
	public class Optional<T>
	{
		public T Value { get; private set; }
		public Exception Fault { get; private set; }

		public bool HasValue {
			get { return Value != null; }
		}

		public bool IsFaulted {
			get { return Fault != null; }
		}

		public Optional (T value)
		{
			Value = value;
		}

		public Optional (Exception fault)
		{
			Fault = fault;
		}
	}
}

