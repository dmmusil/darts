using System;
using System.Collections.Generic;
using Darts.Games.Persistence;
using Xunit;

namespace Darts.Games.Tests
{
    public class CricketGameLoadMemoTests
    {
        [Fact]
        public void Equality()
        {
            var state1 = new CricketState(5);
            PopulateState(state1);

            var state2 = new CricketState(5);
            PopulateState(state2);

            Assert.Equal(state1, state2);
        }

        private readonly Guid PlayerId = Guid.NewGuid();
        private void PopulateState(CricketState state)
        {
            state.Players.Add(new Player{PlayerId = PlayerId});

            state.Turns.Add(new Persistence.Turn(5)
            {
                PlayerId = PlayerId,
                Scores = new List<Score>
                {
                    new Score {Count = 5, Segment = 20}
                },
                Order = 1
            });
        }

        [Fact]
        public void Load_and_Memoize_should_not_alter_state()
        {
            var state = new CricketState(5);
            PopulateState(state);

            var game = new Cricket();
            game.Load(state);

            var memoized = game.Memoize();

            Assert.Equal(state, memoized);
        }
    }
}
