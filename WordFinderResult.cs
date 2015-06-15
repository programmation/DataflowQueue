
namespace DataflowQueue
{
	public class WordFinderResult<T>
	{
		public string Uri { get; private set; }
		public Optional<T> Result { get; private set; }

		public WordFinderResult(string uri, Optional<T> result)
		{
			Uri = uri;
			Result = result;
		}
	}
	
}
