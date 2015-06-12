using System;
using System.Threading.Tasks.Dataflow;
using WordFinderString = DataflowQueue.WordFinderResult<string>;
using WordFinderArray = DataflowQueue.WordFinderResult<string[]>;
using DataflowQueue;

namespace DataflowQueue
{
	public interface INativeReversedWordFinder
	{
		TransformBlock<string, WordFinderString> AsyncStringLoader { get; }
		TransformManyBlock<WordFinderArray, WordFinderString> ReversedWordFinder { get; }

		event Action<string, string> ProgressReporter;
	}
}

