using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Bussines.Helpers
{
    public class DataResponse<T>
    {
        public T Data { get; set; }
        public bool Sussces { get; set; } = true;
        public ActionResult ActionResult { get; set; }
    }
}