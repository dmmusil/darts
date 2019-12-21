using Darts.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Darts.Players.Persistence
{
    public class PlayerState : State
    {
        public PlayerState(int id)
        {
            Id = id;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class PlayerStateMap : IEntityTypeConfiguration<PlayerState>
    {
        public void Configure(EntityTypeBuilder<PlayerState> builder)
        {
            builder.ToTable("Players");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("PlayerId");
            builder.Property(x => x.Username).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(50).IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Username).IsUnique();
        }
    }


}
