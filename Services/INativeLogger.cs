using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac;
using Helpers;

namespace Services
{
	public interface INativeLogger
	{
		void WriteLine (
			long ticks,
			string flag, 
			object location,
			string template, 
			object[] data = null,
			string caller = "",
			int line = 0,
			string file = "",
			int threadId = 0
		);

		int CurrentThreadId ();
	}

}