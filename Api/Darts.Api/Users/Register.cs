using System.Threading.Tasks;
using Darts.Api.Infrastructure;
using Darts.Infrastructure;
using Darts.Players;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

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

            var result = await CommandBus.Send(command);

            return result.ToActionResult();
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
}
