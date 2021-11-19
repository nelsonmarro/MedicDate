﻿using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MedicDate.DataAccess.Helpers
{
    public abstract class OperationResult
    {
        private OperationResult()
        {
        }

        public abstract bool Succeeded { get; }

        public virtual ActionResult SuccessResult { get; init; } = new GenericActionResult(HttpStatusCode.OK, "Operación exitosa");

        public virtual ActionResult ErrorResult { get; init; } = new GenericActionResult(HttpStatusCode.BadGateway, "Operación fallida");

        public static OperationResult Success(HttpStatusCode statusCode,
            object? responseBody = null)
        {
            return new SuccessOperationResultWithMessage(statusCode, responseBody);
        }

        public static OperationResult Success()
        {
            return new SuccessOperationResult();
        }

        public static OperationResult Error(HttpStatusCode statusCode,
            object? responseBody = null)
        {
            return new ErrorOperationResult(statusCode, responseBody);
        }

        public static OperationResult Error(ActionResult errorResult)
        {
            return new ErrorOperationResult(errorResult);
        }

        public sealed class SuccessOperationResultWithMessage : OperationResult
        {
            public SuccessOperationResultWithMessage(HttpStatusCode statusCode,
                object? responseBody = null)
            {
                SuccessResult =
                    GenericActionResultBuilder.BuildResult(statusCode,
                        responseBody);
            }

            public override bool Succeeded => true;
            public override ActionResult SuccessResult { get; init; }
        }

        public sealed class SuccessOperationResult : OperationResult
        {
            public override bool Succeeded => true;
        }

        public sealed class ErrorOperationResult : OperationResult
        {
            public ErrorOperationResult(HttpStatusCode statusCode,
                object? responseBody = null)
            {
                ErrorResult = GenericActionResultBuilder.BuildResult(
                    statusCode, responseBody);
            }

            public ErrorOperationResult(ActionResult errorActionResult)
            {
                ErrorResult = errorActionResult;
            }

            public override bool Succeeded => false;

            public override ActionResult ErrorResult { get; init; }
        }
    }

    public abstract class OperationResult<T>
    {
        public abstract bool Succeeded { get; }
        public virtual ActionResult ErrorResult { get; init; } = new GenericActionResult(HttpStatusCode.BadGateway, "Operación fallida");
        public virtual T? DataResult { get; }

        public static OperationResult<T> Success(T dataResult)
        {
            return new SuccessOperationDataResult(dataResult);
        }

        public static OperationResult<T> Error(HttpStatusCode statusCode,
            object? responseBody = null)
        {
            return new ErrorOperationDataResult(statusCode, responseBody);
        }

        public sealed class SuccessOperationDataResult : OperationResult<T>
        {
            public SuccessOperationDataResult(T dataResult)
            {
                DataResult = dataResult;
            }

            public override bool Succeeded => true;
            public override T DataResult { get; }
        }

        public sealed class ErrorOperationDataResult : OperationResult<T>
        {
            private readonly HttpStatusCode _statusCode;
            private readonly object? _responseBody;

            public ErrorOperationDataResult(HttpStatusCode statusCode,
                object? responseBody)
            {
                _statusCode = statusCode;
                _responseBody = responseBody;
            }

            public override bool Succeeded => false;

            public override ActionResult ErrorResult =>
                GenericActionResultBuilder.BuildResult(_statusCode, _responseBody);
        }
    }
}