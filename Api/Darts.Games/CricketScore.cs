using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Darts.Games
{
    public class CricketScore : IReadOnlyDictionary<Segment, HitCount>
    {
        private readonly Dictionary<Segment, HitCount> Scores = new Dictionary<Segment, HitCount>
        {
            {Segment.From(20), HitCount.From(0)},
            {Segment.From(19), HitCount.From(0)},
            {Segment.From(18), HitCount.From(0)},
            {Segment.From(17), HitCount.From(0)},
            {Segment.From(16), HitCount.From(0)},
            {Segment.From(15), HitCount.From(0)},
            {Segment.From(25), HitCount.From(0)}
        };

        public bool AllClosed => Scores.All(kvp => kvp.Value.Closed);

        public IEnumerator<KeyValuePair<Segment, HitCount>> GetEnumerator()
        {
            return Scores.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Scores.Count;
        public bool ContainsKey(Segment key)
        {
            return Scores.ContainsKey(key);
        }

        public bool TryGetValue(Segment key, out HitCount value)
        {
            return Scores.TryGetValue(key, out value);
        }

        public HitCount this[Segment key] => Scores[key];

        public IEnumerable<Segment> Keys => Scores.Keys;
        public IEnumerable<HitCount> Values => Scores.Values;

        public void AddUpTo(in Segment segment, in TurnMultiplier multiplier)
        {
            Scores[segment] += HitCount.From(multiplier);
            Scores[segment] = HitCount.Min(HitCount.From(3), Scores[segment]);
        }

        public int Points =>
            Scores.Sum(x => x.Value.Points(x.Key));

        public bool HasClosed(Segment segment)
        {
            return Scores[segment].Closed;
        }

        public void Add(Segment segment, TurnMultiplier multiplier)
        {
            Scores[segment] += HitCount.From(multiplier.Value);
        }
    }
}
