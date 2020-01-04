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
    public class GetScore
    {
        private readonly GamesContext Db;

        public GetScore(GamesContext db)
        {
            Db = db;
        }

        [FunctionName("GetScore")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "cricket/{id}/score")] HttpRequest req,
            int id,
            ILogger log)
        {
            var playerTurns = await Db.CricketGames
                .Where(g => g.Id == id)
                .SelectMany(g => g.Turns)
                .Include(t => t.Scores)
                .Select(t => new
                {
                    Scores = t.Scores.Select(s => new {s.Segment, s.Count}),
                    t.PlayerId,
                    t.Order
                })
                .ToListAsync();
            return new OkObjectResult(playerTurns);
        }
    }
}
