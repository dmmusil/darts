using Darts.Api;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlStreamStore;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Darts.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build();
            var settings = new MsSqlStreamStoreSettings(config.GetConnectionString("Database"));
            var store = new MsSqlStreamStore(settings);

            builder.Services.AddSingleton(store);
            builder.Services.AddMediatR(typeof(Startup).Assembly);
        }
    }
}
