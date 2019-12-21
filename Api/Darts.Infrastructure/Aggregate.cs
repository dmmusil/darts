using System.Collections.Generic;

namespace Darts.Infrastructure
{
    public abstract class Aggregate<TState> where TState : State
    {
        private List<Event> _events = new List<Event>();
        public IEnumerable<Event> UncommittedEvents => _events.AsReadOnly();

        public int Id { get; protected set; }


        public abstract void Load(TState state);

        public abstract TState Memoize();
    }

    public abstract class State
    {
        public int Id { get; protected set; }
    }
}
