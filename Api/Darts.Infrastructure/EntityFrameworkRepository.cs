using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OneOf;
using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using OneOf.Types;

namespace Darts.Infrastructure
{
    public abstract class EntityFrameworkRepository<TAggregate, TState, TContext>
        where TAggregate : Aggregate<TState>, new()
        where TState : State
        where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public EntityFrameworkRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<OneOf<TAggregate, None>> Load(int id)
        {
            return Load(x => x.Id == id);
        }

        private static OneOf<TAggregate, None> LoadAggregateFromState(TState result)
        {
            if (result == null)
            {
                return new None();
            }
            var aggregate = new TAggregate();
            aggregate.Load(result);
            return aggregate;
        }

        protected abstract Task<TState> LoadWithIncludes(
            Expression<Func<TState, bool>> selector);

        public async Task<OneOf<TAggregate, None>> Load(
            Expression<Func<TState, bool>> selector)
        {
            var result = await LoadWithIncludes(selector);

            return LoadAggregateFromState(result);
        }

        public async Task<TState> Save(Aggregate<TState> updated)
        {
            updated.State.Handle(updated.UncommittedEvents);
            if (updated.State.Id == default)
            {
                _dbContext.Set<TState>().Add(updated.State);
            }
            try
            {
                await _dbContext.SaveChangesAsync();
                return updated.State;
            }
            catch (Exception ex) when (ex.InnerException is SqlException sql && (sql.Number == 2601 || sql.Number == 2627))
            {
                throw new UniqueConstraintViolationException();
            }
        }
    }

    [Serializable]
    public class UniqueConstraintViolationException : Exception
    {
        public UniqueConstraintViolationException()
        {
        }

        public UniqueConstraintViolationException(string message) : base(message)
        {
        }

        public UniqueConstraintViolationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UniqueConstraintViolationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
