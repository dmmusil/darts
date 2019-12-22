using System.Threading.Tasks;
using Darts.Api.Infrastructure;
using Darts.Players;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Darts.Api.Users
{
    [UsedImplicitly]
    public class Register
    {
        private readonly IMediator _mediator;

        public Register(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("Register")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/register")] HttpRequest req,
            ILogger log)
        {
            var command = await req.ReadJsonBody<RegisterUser.Request>();

            var result = await _mediator.Send(command);

            return result.Match<IActionResult>(
                success => new OkResult(),
                failed => new BadRequestObjectResult(failed.Reason));
        }
    }
}
