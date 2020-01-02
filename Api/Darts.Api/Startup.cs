using Darts.Api;
using Darts.Games;
using Darts.Games.Persistence;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Darts.Players.Persistence.SQL;
using Microsoft.EntityFrameworkCore;
using Darts.Players;
using Microsoft.Extensions.Logging;
using Player = Darts.Players.Player;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Darts.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddMediatR(typeof(Player).Assembly, typeof(Games.Cricket).Assembly);
            builder.Services.AddDbContext<PlayersContext>(options => options.UseSqlServer("Data Source=.;Initial Catalog=Darts;Integrated Security=true"));
            builder.Services.AddDbContext<GamesContext>(options => options.UseSqlServer("Data Source=.;Initial Catalog=Darts;Integrated Security=true").UseLoggerFactory(GetLoggerFactory()));
            builder.Services.AddScoped<PlayerRepository>();
            builder.Services.AddScoped<CricketRepository>();
        }

        private ILoggerFactory GetLoggerFactory() =>
            LoggerFactory.Create(x => x.AddConsole());

    }
}
