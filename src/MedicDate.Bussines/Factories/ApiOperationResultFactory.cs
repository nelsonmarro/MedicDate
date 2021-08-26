using System;
using System.Net;
using System.Text.Json;
using MedicDate.Bussines.Factories.IFactories;
using MedicDate.Bussines.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Bussines.Factories
{
    public class ApiOperationResultFactory : IApiOperationResultFactory
    {
        public ApiOperationResult CreateSuccessApiOperationResult(HttpStatusCode statusCode, object responseBody = null)
        {
            return new ApiOperationResult
            {
                IsSuccess = true,
                SuccessActionResult = BuildGenericActionResult(statusCode, responseBody)
            };
        }

        public ApiOperationResult CreateErrorApiOperationResult(HttpStatusCode statusCode, object responseBody = null)
        {
            return new ApiOperationResult
            {
                IsSuccess = false,
                ErrorActionResult = BuildGenericActionResult(statusCode, responseBody)
            };
        }

        public ApiOperationDataResult<TData> CreateSuccessApiOperationDataResult<TData>(TData resultData)
        {
            return new ApiOperationDataResult<TData>
            {
                IsSuccess = true,
                ResultData = resultData
            };
        }

        public ApiOperationDataResult<TData> CreateErrorApiOperationDataResult<TData>(HttpStatusCode statusCode, object responseBody = null)
        {
            return new ApiOperationDataResult<TData>()
            {
                IsSuccess = false,
                ErrorActionResult = BuildGenericActionResult(statusCode, responseBody)
            };
        }
        public ApiOperationResult CreateErrorApiOperationResult(GenericActionResult errorActionResult)
        {
            return new ApiOperationResult
            {
                IsSuccess = false,
                ErrorActionResult = errorActionResult
            };
        }

        private GenericActionResult BuildGenericActionResult(HttpStatusCode statusCode, object responseBody = null)
        {

            if (responseBody is null)
            {
                return new GenericActionResult(statusCode, null);
            }
            else if (responseBody is string responseBodyString)
            {
                return new GenericActionResult(statusCode, responseBodyString.ToString());
            }
            else
            {
                return new GenericActionResult(
                    statusCode, JsonSerializer.Serialize(responseBody));
            }
        }
    }
}