using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Darts.Infrastructure;
using Darts.Players;
using Darts.Api.Infrastructure;

namespace Darts.Api.Users
{
    public static class Login
    {
        [FunctionName("Login")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")] HttpRequest req,
            ILogger log)
        {
            var loginInfo = await req.ReadJsonBody<Authenticate.Request>();

            var result = await CommandBus.Send(loginInfo);

            return result.ToActionResult();
        }

        public class Authenticate 
        {
            public class Request : Command
            {
                public string Username { get; set; }
                public string Password { get; set; }
            }

            public class Handler
            {
                private readonly SqlStreamStoreRepository repository;

                public Handler(SqlStreamStoreRepository repository)
                {
                    this.repository = repository;
                }

                public async Task Handle(Request command)
                {
                    var user = await repository.Load<Player>(command.Username);
                    if (user.HasValue)
                    {
                        user.Value.Authenticate(command.Password);
                    }
                }
            }
        }
    }
}
