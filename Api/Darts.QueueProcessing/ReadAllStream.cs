using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlStreamStore;
using System.Threading.Tasks;

namespace Darts.QueueProcessing
{
    public static class ReadAllStream
    {
        [FunctionName("ReadAllStream")]
        public static async Task RunAsync(
            [QueueTrigger("event-added", Connection = "AzureWebJobsStorage")]string myQueueItem, 
            ILogger log)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build();
            var settings = new MsSqlStreamStoreSettings(config.GetConnectionString("Database"));
            var store = new MsSqlStreamStore(settings);

            var page = await store.ReadAllForwards(0, 100);
            while (true)
            {
                // handle events
                if (page.IsEnd) break;
                page = await page.ReadNext();
            }
        }
    }
}
