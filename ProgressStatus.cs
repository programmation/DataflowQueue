
namespace DataflowQueue
{
	public class ProgressStatus
	{
		public string Book { get; set; }
		public string Report { get; set; }

		public ProgressStatus (string book, string report)
		{
			Book = book;
			Report = report;
		}
	}
	
}
