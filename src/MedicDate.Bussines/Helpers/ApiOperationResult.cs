using MedicDate.Bussines.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Bussines.Helpers
{
    public class ApiOperationResult : BasicApiOperationResult
    {
        public GenericActionResult SuccessActionResult { get; set; }
    }
}