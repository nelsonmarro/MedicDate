using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Bussines.Helpers
{
    public class BasicApiOperationResult
    {
        public bool IsSuccess { get; set; }
        public GenericActionResult ErrorActionResult { get; set; }
    }
}