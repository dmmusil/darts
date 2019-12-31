using System.Collections.Generic;
using System.Linq;
using Darts.Games.Persistence;

namespace Darts.Games
{
    public class CricketScoreCalculator
    {
        private readonly Dictionary<PlayerId, CricketScore> _playerScores =
            new Dictionary<PlayerId, CricketScore>();

        public bool GameCompleted
        {
            get
            {
                var (player, score) =
                    _playerScores.FirstOrDefault(kvp => kvp.Value.AllClosed);
                if (player == default)
                {
                    return false;
                }

                return score.AllClosed && score.Points >=
                       _playerScores.Max(s => s.Value.Points);
            }
        }

        public bool GameStarted;

        public void AddTurn(Turn turn)
        {
            CheckForDartsThatDontCount(turn);
        }

        private void CheckForDartsThatDontCount(Turn turn)
        {
            var otherPlayersScores =
                _playerScores.Where(s => s.Key != turn.PlayerId).ToList();
            foreach (var (segment, multiplier) in turn.Scores)
            {
                var anyOpponentHasThisNumberClosed =
                    otherPlayersScores.Any(
                        kvp => kvp.Value.HasClosed(segment));
                if (anyOpponentHasThisNumberClosed)
                {
                    var hitCount = _playerScores[turn.PlayerId][segment];
                    var thisPlayerHasTheNumberClosed = hitCount.Closed;
                    if (!thisPlayerHasTheNumberClosed)
                    {
                        _playerScores[turn.PlayerId].AddUpTo(segment, multiplier);
                    }
                }
                else
                {
                    _playerScores[turn.PlayerId].Add(segment, multiplier);
                }
            }
        }

        public void AddPlayer(PlayerId player) => _playerScores.Add(player, new CricketScore());

        public CricketScore CheckScore(PlayerId player) => _playerScores[player];
    }
}
