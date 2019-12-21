using Darts.Api;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Darts.Players.Persistence.SQL;
using Microsoft.EntityFrameworkCore;
using Darts.Players;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Darts.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddMediatR(typeof(Startup).Assembly);
            builder.Services.AddDbContext<PlayersContext>(options => options.UseSqlServer("Data Source=.;Initial Catalog=Darts;Integrated Security=true"));
            builder.Services.AddScoped<PlayerRepository>();
        }
    }
}
