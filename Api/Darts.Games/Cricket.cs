using System.Linq;
using Darts.Games.Persistence;
using Darts.Infrastructure;

namespace Darts.Games
{
    public class Cricket : Aggregate<CricketState>
    {
        private bool Completed => _calculator.GameCompleted;
        private bool Started => _calculator.GameStarted;
        private readonly CricketScoreCalculator _calculator;
        public Cricket()
        {
            _calculator = new CricketScoreCalculator();
        }

        public void AddPlayer(PlayerId player)
        {
            if (!Started)
            {
                _calculator.AddPlayer(player);
            }
        }

        public void AddTurn(Turn turn)
        {
            if (!Completed)
            {
                _calculator.AddTurn(turn);
            }
        }

        public override void Load(CricketState state)
        {
            Id = state.Id;
            foreach (var player in state.Players)
            {
                AddPlayer(PlayerId.From(player.PlayerId));
            }

            foreach (var turn in state.Turns)
            {
                var loading = Turn.ForPlayer(PlayerId.From(turn.PlayerId));
                foreach (var score in turn.Scores)
                {
                    loading.WithScores(Segment.From(score.Segment),
                        TurnMultiplier.From(score.Count));
                }

                loading.WithId(turn.Id);
                AddTurn(loading);
            }
        }

        public override CricketState Memoize()
        {
            var state = new CricketState(Id);
            foreach (var player in _calculator.Players)
            {
                state.Players.Add(new Player { PlayerId = player.Value });
            }

            foreach (var turn in _calculator.Turns)
            {
                var item = new Persistence.Turn(turn.TurnId)
                {
                    PlayerId = turn.PlayerId.Value,
                    Order = state.Turns.Count(t => t.PlayerId == turn.PlayerId.Value) + 1
                };
                foreach (var (segment, count) in turn.Scores)
                {
                    item.Scores.Add(new Score { Count = count.Value, Segment = segment.Value, });
                }
                state.Turns.Add(item);
            }

            return state;
        }
    }
}
