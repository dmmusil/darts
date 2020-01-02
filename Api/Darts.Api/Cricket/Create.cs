using System.Threading.Tasks;
using Darts.Api.Infrastructure;
using Darts.Games;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Darts.Api.Cricket
{
    public class Create
    {
        private readonly IMediator Mediator;

        public Create(IMediator mediator)
        {
            Mediator = mediator;
        }

        [FunctionName("Create")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cricket/create")] HttpRequest req,
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
