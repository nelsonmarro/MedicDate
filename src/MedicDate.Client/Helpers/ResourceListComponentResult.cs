namespace MedicDate.Client.Helpers;

public class ResourceListComponentResult<T> where T : class
{
   public bool Succeded { get; set; }
   public List<T>? ItemList { get; set; }
   public int TotalCount { get; set; }
}