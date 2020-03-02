using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Darts.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Darts.Api
{
    public class MigrateDb
    {
        private readonly DbMigrator _migrator;
        public MigrateDb(DbMigrator migrator) => _migrator = migrator;

        [FunctionName("MigrateDb")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Updating databases.");

            await Task.WhenAll(_migrator.Contexts.Select(c => c.Database.MigrateAsync()));

            return new OkResult();
        }
    }
}
