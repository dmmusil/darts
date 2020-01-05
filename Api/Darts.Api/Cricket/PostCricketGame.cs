using System.Threading.Tasks;
using Darts.Api.Infrastructure;
using Darts.Games;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Darts.Api.Cricket
{
    [UsedImplicitly]
    public class PostCricketGame
    {
        private readonly IMediator Mediator;

        public PostCricketGame(IMediator mediator)
        {
            Mediator = mediator;
        }

        [FunctionName("CreateCricketGame")]
        [UsedImplicitly]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cricket")] HttpRequest req,
            ILogger log)
        {
            var command = await req.ReadJsonBody<CreateCricketGame.Command>();

            var result = await Mediator.Send(command);

            return result.Match<IActionResult>(
                id => new OkObjectResult(id.Value),
                failure => new BadRequestResult());
        }
    }
}
