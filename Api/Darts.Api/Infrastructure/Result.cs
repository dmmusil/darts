using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace Darts.Api.Infrastructure
{
    public class Result
    {
        public string FailureReason { get; }
        public int StatusCode { get; }
        public bool Succeeded { get; }
        private Result(bool success)
        {
            Succeeded = success;
        }

        private Result(bool success, string failureReason, in int statusCode) : this(success)
        {
            FailureReason = failureReason;
            StatusCode = statusCode;
        }

        private Result(bool success, object value)
        {
            Succeeded = success;
            Value = value;
        }

        public static Result Success() => new Result(true);

        public object Value { get; private set; }
        private bool HasValue => Value != null;
        public static Result Failure(string failureReason, int statusCode)
        {
            return new Result(false, failureReason, statusCode);
        }

        public IActionResult ToActionResult(object result)
        {
            if (Succeeded)
            {
                return new OkObjectResult(result);
            }

            return HandleError();
        }

        private IActionResult HandleError()
        {
            switch (StatusCode)
            {
                case 400:
                    return new BadRequestErrorMessageResult(FailureReason);
                default:
                    return new InternalServerErrorResult();
            }
        }

        public IActionResult ToActionResult()
        {
            if (Succeeded)
            {
                return HasValue ? (IActionResult)new OkObjectResult(Value) : new OkResult();
            }

            return HandleError();
        }

        public static Result Success(object value)
        {
            return new Result(true, value);
        }
    }
}
