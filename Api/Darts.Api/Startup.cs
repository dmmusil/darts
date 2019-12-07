using Darts.Api;
using Darts.Api.Users;
using Darts.Infrastructure;
using Darts.Players;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using SqlStreamStore;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Darts.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            TypeMapper.RegisterMapping("UserRegistered", typeof(UserRegistered));

            CommandPipeline.Initialize(new InMemoryStreamStore());
        }
    }
}
