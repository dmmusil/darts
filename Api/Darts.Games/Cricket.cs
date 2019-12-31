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
            throw new System.NotImplementedException();
        }

        public override CricketState Memoize()
        {
            throw new System.NotImplementedException();
        }
    }
}
