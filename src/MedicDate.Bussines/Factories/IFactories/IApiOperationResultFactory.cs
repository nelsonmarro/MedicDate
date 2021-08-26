using System.Net;
using MedicDate.Bussines.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Bussines.Factories.IFactories
{
    public interface IApiOperationResultFactory
    {
        ApiOperationResult CreateSuccessApiOperationResult(HttpStatusCode statusCode, object responseBody = null);

        ApiOperationResult CreateErrorApiOperationResult(HttpStatusCode statusCode, object responseBody = null);

        ApiOperationResult CreateErrorApiOperationResult(GenericActionResult errorActionResult);

        ApiOperationDataResult<TData> CreateSuccessApiOperationDataResult<TData>(TData resultData);

        ApiOperationDataResult<TData> CreateErrorApiOperationDataResult<TData>(HttpStatusCode statusCode, object responseBody = null);
    }
}