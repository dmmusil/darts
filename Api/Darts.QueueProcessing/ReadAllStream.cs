using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlStreamStore;
using System.Threading.Tasks;

namespace Darts.QueueProcessing
{
    public class ReadAllStream
    {
        private readonly IMediator _mediator;
        private readonly MsSqlStreamStore _store;

        public ReadAllStream(IMediator mediator, MsSqlStreamStore store)
        {
            _mediator = mediator;
            _store = store;
        }

        [FunctionName("ReadAllStream")]
        public async Task RunAsync(
            [QueueTrigger("event-added", Connection = "AzureWebJobsStorage")]string myQueueItem,
            ILogger log)
        {
            var page = await _store.ReadAllForwards(0, 100);
            while (true)
            {
                foreach (var message in page.Messages)
                {

                    await _mediator.Publish(await message.ToNotification());
                }

                if (page.IsEnd) break;
                page = await page.ReadNext();
            }
        }
    }
}
