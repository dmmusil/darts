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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cricket/add-turn")] HttpRequest req,
            ILogger log)
        {
            var json = await req.ReadAsStringAsync();

            var command = new AddCricketTurn.Command(Turn.FromJson(json),
                int.Parse(JObject.Parse(json)["gameId"].ToString()));

            var result = await Mediator.Send(command);

            return result.Match<IActionResult>(
                success => new OkResult(),
                none => new BadRequestResult());
        }
    }
}
