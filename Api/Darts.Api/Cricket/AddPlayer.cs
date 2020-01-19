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
    public class AddPlayer
    {
        private readonly IMediator Mediator;

        public AddPlayer(IMediator mediator)
        {
            Mediator = mediator;
        }

        [FunctionName("AddCricketPlayer")]
        [UsedImplicitly]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",
                Route = "cricket/{gameId}/players")]
            HttpRequest req,
            int gameId,
            ILogger log)
        {
            var command = await req.ReadJsonBody<AddCricketPlayer.Command>();
            command.ForGame(gameId);

            await Mediator.Send(command);

            return new OkResult();
        }
    }
}
