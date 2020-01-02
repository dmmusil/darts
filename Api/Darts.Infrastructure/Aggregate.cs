using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Darts.Infrastructure
{
    public abstract class Aggregate<TState> where TState : State
    {
        private readonly List<Event> _events = new List<Event>();
        private bool Loading;
        public IEnumerable<Event> UncommittedEvents => _events.AsReadOnly();

        public int Id { get; protected set; }
        public TState State { get; protected set; }

        protected void Apply(Event e)
        {
            if (Loading) return;
            ApplyEvent(e);
            _events.Add(e);
        }
        protected abstract void ApplyEvent(Event e);

        public void Load(TState state)
        {
            Loading = true;
            State = state;
            LoadInternal(state);
            Loading = false;
        }
        protected abstract void LoadInternal(TState state);
    }

    public abstract class State
    {
        protected State(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public abstract void Handle(IEnumerable<Event> events);
    }
}
