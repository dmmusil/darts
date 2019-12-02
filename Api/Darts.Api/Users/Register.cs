using System;
using System.Threading.Tasks;
using System.Web.Http;
using Darts.Infrastructure;
using Darts.Players;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Darts.Api.Users
{
    public static class Register
    {
        [FunctionName("Register")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/register")] HttpRequest req,
            ILogger log)
        {
            var command = await req.ReadJsonBody<RegisterUser.Request>();

            var result = await CommandPipeline.Send(command);

            return result.ToActionResult();
        }
    }

    public static class CommandPipeline
    {
        public static async Task<Result> Send(Command command)
        {
            try
            {
                switch (command)
                {
                    case RegisterUser.Request c:
                        await new RegisterUser.Handler().Handle(c);
                        return Result.Success;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Can't handle command of type ${command.GetType().Name}");
                }
            }
            catch (DomainException e)
            {
                return Result.Failure(e.FailureReason, e.StatusCode);
            }
        }
    }



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

        public static Result Success => new Result(true);

        public static Result Failure(string failureReason, int statusCode)
        {
            return new Result(false, failureReason, statusCode);
        }


        public IActionResult ToActionResult()
        {
            if (Succeeded)
            {
                return new OkResult();
            }

            switch (StatusCode)
            {
                case 400:
                    return new BadRequestErrorMessageResult(FailureReason);
                default:
                    return new InternalServerErrorResult();
            }
        }
    }


    public static class RegisterUser
    {
        public class Request : Command
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        public class Handler
        {
            public Task Handle(Request request)
            {
                var user = new Player();
                user.Register(request.Username, request.Password, request.Email);
                return Task.CompletedTask;
            }
        }
    }

    public static class HttpRequestMessageExtensions
    {
        public static async Task<T> ReadJsonBody<T>(this HttpRequest req)
        {
            var json = await req.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
