using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Diagnostics;
using Services;
using Foundation;
using UIKit;

namespace Dataflow.iOS.Services
{
	/// <summary>
	/// Native Logger
	/// </summary>
	public class NativeLogger
		: INativeLogger
	{
		public NativeLogger ()
		{
		}

		public void WriteLine (
			long ticks,
			string flag, 
			object location,
			string template, 
			object[] data = null,
			string caller = "",
			int line = 0,
			string file = "",
			int threadId = 0
		)
		{
			var stamp = new DateTime (ticks, DateTimeKind.Utc);
			var typeName = location.GetType ().FullName;
			var message = template;
			if (data != null) {
				message = String.Format (template, data);
			}
			var debugMessage = String.Format ("[{5:O}][{0}{1:D5}][{2} L{3}]: {4}", flag, threadId, typeName + "." + caller, line, message, stamp);
			Console.WriteLine (debugMessage);
		}

		public int CurrentThreadId ()
		{
			return System.Threading.Thread.CurrentThread.ManagedThreadId;
		}
	}
}
