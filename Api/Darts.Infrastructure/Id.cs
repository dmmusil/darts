using System;
using ValueOf;

namespace Darts.Infrastructure
{
    public class Id : ValueOf<Guid, Id>
    {
        public Id()
        {
            Value = Guid.NewGuid();
        }

        protected override void Validate()
        {
            if (Value == Guid.Empty)
                throw new ArgumentException(nameof(Value), "Cannot use Guid.Empty");
        }

        public static implicit operator string(Id id) => id.Value.ToString("N");
        public static implicit operator Id(string id) => From(Guid.Parse(id));
    }
}
