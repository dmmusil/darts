using Darts.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Darts.Players.Persistence.SQL
{
    public class PlayersContext : DbContext
    {
        public PlayersContext(DbContextOptions<PlayersContext> options)
            : base(options)
        {
        }

        public DbSet<PlayerState> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlayerState).Assembly);
        }
    }

    public class PlayContextDesignTimeThing : IDesignTimeDbContextFactory<PlayersContext>
    {
        public PlayersContext CreateDbContext(string[] args)
        {
            return new PlayersContext(
                new DbContextOptionsBuilder<PlayersContext>()
                .UseSqlServer("Data Source=.;Initial Catalog=Darts;Integrated Security=true")
                .Options
            );
        }
    }
}
