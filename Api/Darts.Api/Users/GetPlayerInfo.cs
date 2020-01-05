using System;
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
    public class GetPlayerInfo
    {
        private readonly PlayersContext Db;

        public GetPlayerInfo(PlayersContext db)
        {
            Db = db;
        }

        [FunctionName("GetPlayerInfo")]
        [UsedImplicitly]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route =
                "users/{id:guid}")]
            HttpRequestMessage req,
            Guid id
        )
        {
            var user = await Db.Players
                .Where(u => u.AuthToken == id)
                .Select(u => new { Name = u.Username })
                .SingleOrDefaultAsync();
            return new OkObjectResult(user);
        }
    }
}
