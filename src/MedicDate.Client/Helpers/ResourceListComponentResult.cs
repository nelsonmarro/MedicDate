namespace MedicDate.Client.Helpers
{
    public class ResourceListComponentResult<T> where T : class
    {
        public bool Succeded { get; set; }
        public IEnumerable<T> ItemList { get; set; } = new List<T>();
        public int TotalCount { get; set; }
    }
}
