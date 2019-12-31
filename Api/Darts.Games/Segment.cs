using System;
using ValueOf;

namespace Darts.Games
{
    public class Segment : ValueOf<int, Segment>
    {
        protected override void Validate()
        {
            if (Value < 1 || Value > 20 && Value != 25)
            {
                throw new ArgumentException("Invalid cricket segment");
            }
        }
    }
}
