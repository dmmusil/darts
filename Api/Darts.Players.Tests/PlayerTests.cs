using Xbehave;
using Xunit;

namespace Darts.Players.Tests
{
    public class PlayerTests
    {
        private readonly Player Player = new Player();

        [Scenario]
        public void Registration()
        {
            "A newly registered user".x(x =>
                Player.Register("dylan", "password", "dylan@musil.dev"));

            "Should be able to authenticate".x(x =>
            {
                var result = Player.Authenticate("password");
                Assert.True(result.IsT0);
                Assert.NotNull(result.AsT0);
            });
        }

        [Scenario]
        public void IncorrectPassword()
        {
            Player.Register("dylan", "password", "dylan@musil.dev");

            "A user providing an incorrect password is not authenticated".x(x =>
            {
                var result = Player.Authenticate("incorrect");
                Assert.True(result.IsT1);
                Assert.False(result.AsT1);
            });
        }
    }
}
