using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OneOf;
using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Darts.Infrastructure
{
    public class EntityFrameworkRepository<TAggregate, TState, TContext> 
        where TAggregate : Aggregate<TState>, new()
        where TState: State 
        where TContext:DbContext
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

        public async Task<OneOf<TAggregate, None>> Load(Expression<Func<TState, bool>> selector)
        {
            var result = await _dbContext.Set<TState>().SingleOrDefaultAsync(selector);
            return LoadAggregateFromState(result);
        }

        public async Task Save(Aggregate<TState> updated)
        {
            _dbContext.Attach(updated.Memoize());

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex) when (ex?.InnerException is SqlException sql && (sql.Number == 2601 || sql.Number == 2627))
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

    public class None
    {
    }

    public class MessageTypeNotRegisteredException : Exception
    {
        public MessageTypeNotRegisteredException(string typeName) : base($"{typeName} is not registered.")
        {
        }
    }

    public class Maybe<T> where T : class
    {
        public T Value { get; }
        public bool HasValue => Value != null;

        private Maybe(T value)
        {
            Value = value;
        }

        public static Maybe<T> From(T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> None => new Maybe<T>(null);
    }
}
