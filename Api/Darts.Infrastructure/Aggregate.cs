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
        public string Identifier { get; protected set; }
        public IEnumerable<Event> UncommittedEvents => _events.AsReadOnly();
        /// <summary>
        /// ExpectedVersion.NoStream is -1 so it will trigger creation of a new stream
        /// </summary>
        public int Version { get; private set; } = -1; 
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
