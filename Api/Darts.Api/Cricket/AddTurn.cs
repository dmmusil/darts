using System.Threading.Tasks;
using Darts.Games;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Darts.Api.Cricket
{
    [UsedImplicitly]
    public class AddTurn
    {
        private readonly IMediator Mediator;

        public AddTurn(IMediator mediator)
        {
            Mediator = mediator;
        }

        [FunctionName("AddCricketTurn")]
        [UsedImplicitly]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cricket/{gameId}/turns")] HttpRequest req,
            int gameId,
            ILogger log)
        {
            var json = await req.ReadAsStringAsync();

            var command = new AddCricketTurn.Command(Turn.FromJson(json), gameId);

            var result = await Mediator.Send(command);

            return result.Match<IActionResult>(
                success => new OkResult(),
                none => new BadRequestResult());
        }
    }
}
