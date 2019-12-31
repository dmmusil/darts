using ValueOf;

namespace Darts.Games
{
    public class HitCount : ValueOf<int, HitCount>
    {
        public bool Closed => Value >= 3;
        public int Points(Segment s) => Value > 3 ? (Value - 3) * s.Value : 0;

        public static HitCount Min(HitCount a, HitCount b)
        {
            return a.Value > b.Value ? b : a;
        }

        public static HitCount From(TurnMultiplier multiplier) =>
            From(multiplier.Value);

        public static HitCount operator +(HitCount a, HitCount b) =>
            From(a.Value + b.Value);
    }
}
