using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Bussines.Helpers
{
    public class ApiOperationDataResult<T> : BasicApiOperationResult
    {
        public T ResultData { get; set; }
    }
}