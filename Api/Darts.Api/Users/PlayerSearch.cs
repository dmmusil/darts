using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Darts.Players.Persistence.SQL;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;

namespace Darts.Api.Users
{
    [UsedImplicitly]
    public class PlayerSearch
    {
        private readonly PlayersContext Db;

        public PlayerSearch(PlayersContext db)
        {
            Db = db;
        }

        [FunctionName("PlayerSearch")]
        [UsedImplicitly]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route =
                "users")]
            HttpRequestMessage req
        )
        {
            var query = req.RequestUri.ParseQueryString()["search"];

            var users = Db.Players.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query))
            {
                users = users.Where(u => u.Username.Contains(query));
            }

            var result = await users
                .Select(u => new { Name = u.Username, PlayerId = u.AuthToken })
                .ToListAsync();

            return new OkObjectResult(result);
        }
    }
}
