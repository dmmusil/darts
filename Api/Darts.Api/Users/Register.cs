using System.Threading.Tasks;
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
            var command = await req.ReadJsonBody<RegisterUser.Command>();

            await new RegisterUser.Handler().Handle(command);

            return new OkObjectResult(command);
        }
    }



    public static class RegisterUser
    {
        public class Command
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        public class Handler
        {
            public Task Handle(Command command)
            {
                var user = new Player();
                user.Register(command.Username, command.Password, command.Email);
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
