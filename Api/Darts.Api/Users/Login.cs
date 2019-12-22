using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Darts.Players;
using Darts.Api.Infrastructure;
using MediatR;
using JetBrains.Annotations;

namespace Darts.Api.Users
{
    public class Login
    {
        private readonly IMediator _mediator;

        public Login(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("Login")]
        [UsedImplicitly]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")] HttpRequest req,
            ILogger log)
        {
            var loginInfo = await req.ReadJsonBody<Authenticate.Request>();

            var result = await _mediator.Send(loginInfo);

            return result.Match<IActionResult>(
                success => new OkObjectResult((string)success.AuthToken),
                failure => new BadRequestObjectResult("Incorrect username or password.")
            );
        }
    }
}
