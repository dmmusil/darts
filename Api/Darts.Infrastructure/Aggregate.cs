using System.Collections.Generic;

namespace Darts.Infrastructure
{
    public abstract class Aggregate
    {
        private readonly List<Event> _events = new List<Event>();
        protected abstract void Apply(Event e);

        protected void ApplyEvent(Event e)
        {
            _events.Add(e);
            Apply(e);
        }
        public Id Identifier { get; protected set; }
        public IEnumerable<Event> UncommittedEvents => _events.AsReadOnly();
        public int Version { get; private set; }
        public void Load(IEnumerable<Event> events)
        {
            foreach (var e in events)
            {
                Apply(e);
                Version++;
            }
        }


    }
}
