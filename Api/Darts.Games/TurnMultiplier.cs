using System;
using ValueOf;

namespace Darts.Games
{
    public class TurnMultiplier : ValueOf<int, TurnMultiplier>
    {
        protected override void Validate()
        {
            if (Value < 0 || Value > 9)
            {
                throw new ArgumentException("Invalid turn multiplier");
            }
        }

        public static TurnMultiplier operator +(TurnMultiplier current, TurnMultiplier incoming)
        {
            return From(current.Value + incoming.Value);
        }
    }
}
