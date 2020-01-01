using System.Collections.Generic;
using Darts.Games.Persistence;

namespace Darts.Games
{
    public class Turn
    {
        public int TurnId { get; private set; }
        public PlayerId PlayerId { get; }
        private Turn(PlayerId playerId)
        {
            PlayerId = playerId;
        }

        public Turn WithScores(Segment segment, TurnMultiplier turnMultiplier)
        {
            EnsureSegment(segment);
            Scores[segment] += turnMultiplier;
            return this;
        }

        private void EnsureSegment(Segment segment)
        {
            if (Scores.ContainsKey(segment))
                return;
            Scores[segment] = TurnMultiplier.From(0);
        }

        public Dictionary<Segment, TurnMultiplier> Scores { get; } =
            new Dictionary<Segment, TurnMultiplier>();

        public static Turn ForPlayer(PlayerId playerId)
        {
            return new Turn(playerId);
        }

        public void WithId(in int turnId)
        {
            TurnId = turnId;
        }
    }
}
