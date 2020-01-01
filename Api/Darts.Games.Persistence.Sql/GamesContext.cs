using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Darts.Games.Persistence.Sql
{
    public class GamesContext : DbContext
    {
        public GamesContext(DbContextOptions<GamesContext> options)
            : base(options)
        {
        }

        [UsedImplicitly] public DbSet<CricketState> CricketGames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }

    public class PlayContextDesignTimeThing : IDesignTimeDbContextFactory<GamesContext>
    {
        public GamesContext CreateDbContext(string[] args)
        {
            return new GamesContext(
                new DbContextOptionsBuilder<GamesContext>()
                    .UseSqlServer("Data Source=.;Initial Catalog=Darts;Integrated Security=true")
                    .Options
            );
        }
    }

    public class ScoreMap : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.HasKey(x => new { x.TurnId, x.Segment });

            builder.HasOne(x => x.Turn)
                .WithMany(x => x.Scores)
                .HasForeignKey(x => x.TurnId);

            builder.HasIndex(x => new { x.TurnId, x.Segment }).IsUnique();
        }
    }

    public class CricketGameMap : IEntityTypeConfiguration<CricketState>
    {
        public void Configure(EntityTypeBuilder<CricketState> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("GameId");

            builder.HasMany(x => x.Players)
                .WithOne(x => x.Game)
                .HasForeignKey(x => x.GameId);

            builder.HasMany(x => x.Turns)
                .WithOne(x => x.Game)
                .HasForeignKey(x => x.GameId);
        }
    }

    public class TurnMap : IEntityTypeConfiguration<Turn>
    {
        public void Configure(EntityTypeBuilder<Turn> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("TurnId");

            builder.HasOne(x => x.Player)
                .WithMany()
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.PlayerId, x.GameId, x.Order });
        }
    }

}
