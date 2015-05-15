using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac;
using System.Threading.Tasks.Dataflow;
using Helpers;
using Services;
using DataflowQueue;

namespace Services
{
	public class LogData
	{
		public long Ticks { get; private set; }

		public string Flag { get; private set; }

		public object Location { get; private set; }

		public string Template { get; private set; }

		public object[] Data { get; private set; }

		public string Caller { get; private set; }

		public int Line { get; private set; }

		public string File { get; private set; }

		public int ThreadId { get; private set; }

		public LogData (string flag, 
			object location,
			string template, 
			object[] data = null,
			string caller = "",
			int line = 0,
			string file = "",
			int threadId = 0
		)
		{
			Ticks = DateTime.UtcNow.Ticks;
			Flag = flag;
			Location = location;
			Template = template;
			Data = data;
			Caller = caller;
			Line = line;
			File = file;
			ThreadId = threadId;
		}
	}

	public interface ILogger
	{
		void Debug (object location, string template, object data1, 
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "");

		void Debug (object location, string template, object data1, object data2,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "");

		void Debug (object location, string template, object data1, object data2, object data3,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "");

		void Debug (object location, string template, object data1, object data2, object data3, object data4,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "");

		void Debug (object location, string template, object[] data = null, 
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "");

		void Info (object location, string template, object data1, 
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "");

		void Info (object location, string template, object data1, object data2,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "");

		void Info (object location, string template, object data1, object data2, object data3,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "");

		void Info (object location, string template, object data1, object data2, object data3, object data4,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "");

		void Info (object location, string template, object[] data = null,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "");
	}

	/// <summary>
	/// Logger.
	/// Offers debug logging (only available if DEBUG)
	/// and Info logging (available on all configurations).
	/// </summary>
	public class Logger
		: ILogger
	{
		#pragma warning disable 414
		// Don't warn about variable usage
		private Dictionary<string, bool> _excluded;
		#pragma warning restore 414

		private IPclBlockingCollection<LogData> _log;
		private readonly INativeLogger _nativeLogger;

		private readonly ActionBlock<LogData> _loggingQueue;

		public Logger ()
		{
			_excluded = new Dictionary<string, bool> ();
			_log = IoC.Container.Resolve<IPclBlockingCollection<LogData>> ();
			_nativeLogger = IoC.Container.Resolve<INativeLogger> ();

			_loggingQueue = new ActionBlock<LogData> (logEntry => {
				_nativeLogger.WriteLine (logEntry.Ticks, logEntry.Flag, logEntry.Location, logEntry.Template, logEntry.Data, logEntry.Caller, logEntry.Line, logEntry.File, logEntry.ThreadId);
			});

//			Start ();
		}

		private void Start ()
		{
			#pragma warning disable 414
			// A simple blocking consumer with no cancellation.
			Task.Run (() => {
				while (!_log.IsCompleted) {
					LogData logEntry = null;
					// Blocks if number.Count == 0 
					// IOE means that Take() was called on a completed collection. 
					// Some other thread can call CompleteAdding after we pass the 
					// IsCompleted check but before we call Take.  
					// In this example, we can simply catch the exception since the  
					// loop will break on the next iteration. 
					try {
						logEntry = _log.Take ();
					} catch (InvalidOperationException ex) {
						// do nothing
						var test = 1;
						//						Debug.WriteLine ("LOGGER EXCEPTION: {0}", ex);
					} catch (Exception ex) {
						// do nothing
						var test = 1;
						//						Debug.WriteLine ("LOGGER EXCEPTION: {0}", ex);
					}
					if (logEntry != null) {
						_nativeLogger.WriteLine (logEntry.Ticks, logEntry.Flag, logEntry.Location, logEntry.Template, logEntry.Data, logEntry.Caller, logEntry.Line, logEntry.File, logEntry.ThreadId);
					}
				}
			});
			#pragma warning restore 414
		}

		public void Debug (object location, string template, 
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			Debug (location, template, new object[0], caller, line, file);
		}

		public void Debug (object location, string template, object data1, 
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			Debug (location, template, new object[] { data1 }, caller, line, file);
		}

		public void Debug (object location, string template, object data1, object data2,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			Debug (location, template, new object[] { data1, data2 }, caller, line, file);
		}

		public void Debug (object location, string template, object data1, object data2, object data3,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			Debug (location, template, new object[] { data1, data2, data3 }, caller, line, file);
		}

		public void Debug (object location, string template, object data1, object data2, object data3, object data4,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			Debug (location, template, new object[] { data1, data2, data3, data4 }, caller, line, file);
		}

		public void Debug (object location, string template, object[] data, 
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			#if DEBUG
			Write (new LogData ("DBUG", location, template, data, caller, line, file, _nativeLogger.CurrentThreadId ()));
			#endif
		}

		public void Info (object location, string template, 
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			Info (location, template, new object[0], caller, line, file);
		}

		public void Info (object location, string template, object data1, 
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			Info (location, template, new object[] { data1 }, caller, line, file);
		}

		public void Info (object location, string template, object data1, object data2,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			Info (location, template, new object[] { data1, data2 }, caller, line, file);
		}

		public void Info (object location, string template, object data1, object data2, object data3,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			Info (location, template, new object[] { data1, data2, data3 }, caller, line, file);
		}

		public void Info (object location, string template, object data1, object data2, object data3, object data4,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			Info (location, template, new object[] { data1, data2, data3, data4 }, caller, line, file);
		}

		public void Info (object location, string template, object[] data,
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0,
			[CallerFilePath] string file = "")
		{
			Write (new LogData ("INFO", location, template, data, caller, line, file, _nativeLogger.CurrentThreadId ()));
		}

		private void Write (LogData logEntry)
		{
			_loggingQueue.Post (logEntry);
//			_log.Add (logEntry);
		}
	}
}
