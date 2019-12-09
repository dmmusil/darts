using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace Darts.Api.Infrastructure
{
    public static class HttpRequestMessageExtensions
    {
        public static async Task<T> ReadJsonBody<T>(this HttpRequest req)
        {
            var json = await req.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
