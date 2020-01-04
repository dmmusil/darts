using System.Linq;
using System.Threading.Tasks;
using Darts.Games.Persistence;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Darts.Api.Cricket
{
    [UsedImplicitly]
    public class GetGame
    {
        private readonly GamesContext Db;

        public GetGame(GamesContext db)
        {
            Db = db;
        }

        [FunctionName("GetGame")]
        [UsedImplicitly]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "cricket/{id}")] HttpRequest req,
            int id,
            ILogger log)
        {
            var gameInfo = await Db.CricketGames
                .Where(g => g.Id == id)
                .Select(g => new { Players = g.Players.Select(p => p.PlayerId) })
                .SingleOrDefaultAsync();

            return new OkObjectResult(gameInfo);
        }
    }
}
