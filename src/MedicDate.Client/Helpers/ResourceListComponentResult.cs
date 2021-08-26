using System.Collections.Generic;

namespace MedicDate.Client.Helpers
{
	public class ResourceListComponentResult<T> where T : class
	{
		public bool Succeded { get; set; }
		public IEnumerable<T> ItemList { get; set; }
		public int TotalCount { get; set; }
	}
}
