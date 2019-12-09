using Darts.Api;
using Darts.Api.Infrastructure;
using Darts.Infrastructure;
using Darts.Players;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SqlStreamStore;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Darts.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            TypeMapper.RegisterMapping("UserRegistered", typeof(UserRegistered));

            var config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build();
            var settings = new MsSqlStreamStoreSettings(config.GetConnectionString("Database"));
            var store = new MsSqlStreamStore(settings);
            store.CreateSchema().GetAwaiter().GetResult();

            CommandBus.Initialize(store);
        }
    }
}
