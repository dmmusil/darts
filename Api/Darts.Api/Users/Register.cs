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
using SqlStreamStore;
using SqlStreamStore.Infrastructure;
using static Darts.Api.Users.Login;

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
        private static StreamStoreBase Store;

        public static void Initialize(StreamStoreBase store)
        {
            Store = store;
        }

        public static async Task<Result> Send(Command command)
        {
            var repository = new SqlStreamStoreRepository(Store);
            try
            {
                switch (command)
                {
                    case RegisterUser.Request c:
                        await new RegisterUser.Handler(repository).Handle(c);
                        return Result.Success();
                    case Authenticate.Request c:
                        await new Authenticate.Handler(repository).Handle(c);
                        return Result.Success(new { Token = (SecureUsername)c.Username });
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
            private readonly SqlStreamStoreRepository _repository;

            public Handler(SqlStreamStoreRepository repository)
            {
                _repository = repository;
            }

            public Task Handle(Request request)
            {
                var user = new Player();
                user.Register(request.Username, request.Password, request.Email);
                return _repository.Save(user);
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
