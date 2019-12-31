using System;
using Darts.Games.Persistence;
using Xbehave;
using Xunit;

namespace Darts.Games.Tests
{
    public class CricketGameCalculatorTests
    {
        private readonly CricketScoreCalculator _calc = new CricketScoreCalculator();
        [Scenario]
        public void Darts_thrown_at_closed_numbers_do_not_count()
        {
            var player1 = PlayerId.From(Guid.NewGuid());
            var player2 = PlayerId.From(Guid.NewGuid());
            "With 2 players in a cricket game".x(x =>
            {
                _calc.AddPlayer(player1);
                _calc.AddPlayer(player2);
            });

            "when the first player hits 5 20s".x(x =>
                _calc.AddTurn(Turn.ForPlayer(player1)
                    .WithScores(Segment.From(20), TurnMultiplier.From(5))));

            "the score should be 5 20s.".x(x =>
                Assert.Equal(HitCount.From(5), _calc.CheckScore(player1)[Segment.From(20)]));

            "If player 2 hits 5 20s".x(x => _calc.AddTurn(
                Turn.ForPlayer(player2)
                    .WithScores(Segment.From(20), TurnMultiplier.From(5))));

            "they should only get credit for 3.".x(x =>
                Assert.Equal(HitCount.From(3), _calc.CheckScore(player2)[Segment.From(20)]));
        }

        [Scenario]
        public void Game_completes_only_when_player_has_point_lead()
        {
            var player1 = PlayerId.From(Guid.NewGuid());
            var player2 = PlayerId.From(Guid.NewGuid());
            "With 2 players in a cricket game".x(x =>
            {
                _calc.AddPlayer(player1);
                _calc.AddPlayer(player2);
            });

            "The first player hits 9 20s".x(x =>
                _calc.AddTurn(Turn.ForPlayer(player1)
                    .WithScores(Segment.From(20), TurnMultiplier.From(9))));

            "The first player has 120 points".x(x =>
                Assert.Equal(120, _calc.CheckScore(player1).Points));

            "The second player closes all the whole board".x(x =>
            {
                _calc.AddTurn(Turn.ForPlayer(player2)
                    .WithScores(Segment.From(20), TurnMultiplier.From(3))
                    .WithScores(Segment.From(19), TurnMultiplier.From(3))
                    .WithScores(Segment.From(18), TurnMultiplier.From(3))
                    .WithScores(Segment.From(17), TurnMultiplier.From(3))
                    .WithScores(Segment.From(16), TurnMultiplier.From(3))
                    .WithScores(Segment.From(15), TurnMultiplier.From(3))
                    .WithScores(Segment.From(25), TurnMultiplier.From(3)));
            });

            "The game is not complete".x(x =>
                Assert.False(_calc.GameCompleted));

            "The second player hits 6 bulls".x(x =>
                _calc.AddTurn(Turn.ForPlayer(player2)
                    .WithScores(Segment.From(25), TurnMultiplier.From(6))));

            "The game is complete".x(x => Assert.True(_calc.GameCompleted));
        }
    }
}
