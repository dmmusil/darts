using System;
using System.Collections.Generic;
using System.Linq;
using Darts.Games.Persistence;
using Newtonsoft.Json.Linq;

namespace Darts.Games
{
    public class Turn
    {
        public static Turn FromJson(string json)
        {
            var jObject = JObject.Parse(json)["turn"];
            var turn = ForPlayer(jObject["playerId"].ToString());

            return jObject["scores"].ToObject<Dictionary<string, int>>()
                .Aggregate(turn,
                    (current, score) =>
                        current.WithScores(
                            Segment.From(Convert.ToInt32(score.Key)),
                            TurnMultiplier.From(score.Value)));
        }

        public int TurnId { get; private set; }
        public PlayerId PlayerId { get; private set; }
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
