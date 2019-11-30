using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Darts.Api
{
    public static class RegisterUser
    {
        [FunctionName("RegisterUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
                var requestBody = await ;
                return new OkObjectResult(requestBody);
        }
    }

    public static class HttpRequestMessageExtensions{
        public static async Task<T> ReadJsonBody<T>(this HttpRequest req)
        {
            var json = await req.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
