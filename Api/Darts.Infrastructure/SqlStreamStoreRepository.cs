using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SqlStreamStore.Infrastructure;
using SqlStreamStore.Streams;

namespace Darts.Infrastructure
{
    public class SqlStreamStoreRepository : IRepository
    {
        private readonly StreamStoreBase _store;

        public SqlStreamStoreRepository(StreamStoreBase store) => _store = store;

        public async Task<Maybe<T>> Load<T>(string id) where T : Aggregate, new()
        {
            var messages = new List<Event>();

            ReadStreamPage page;
            do
            {
                page = await _store.ReadStreamForwards(new StreamId(id), StreamVersion.Start, 100);
                messages.AddRange(await Task.WhenAll(page.Messages.Select(async m =>
                    (Event)JsonConvert.DeserializeObject(await m.GetJsonData(),
                        TypeMapper.GetClrTypeForMessageType(m.Type)))));
                page = await page.ReadNext();
            }
            while (!page.IsEnd);

            if (!messages.Any())
            {
                return Maybe<T>.None;
            }

            var result = new T();
            result.Load(messages);
            return Maybe<T>.From(result);
        }

        public Task Save(Aggregate updated)
        {
            var streamId = new StreamId(updated.Identifier);
            var newStreamMessages = updated.UncommittedEvents
                .Select(e => new NewStreamMessage(Guid.NewGuid(), TypeMapper.GetMessageNameForClrType(e.GetType()),
                    JsonConvert.SerializeObject(e)))
                .ToArray();

            return _store.AppendToStream(streamId, updated.Version, newStreamMessages);
        }
    }

    public interface IUnitOfWork
    {
        void Attach(Aggregate updated);
        Task Save();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Id, Aggregate> _trackedAggregates = new Dictionary<Id, Aggregate>();
        public void Attach(Aggregate updated)
        {
            _trackedAggregates[updated.Identifier] = updated;

        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }

    public static class TypeMapper
    {
        private static readonly Dictionary<string, Type> MessageNameToEventType = new Dictionary<string, Type>();

        public static void RegisterMapping(string messageType, Type clrType) =>
            MessageNameToEventType[messageType] = clrType;

        public static Type GetClrTypeForMessageType(string messageType) =>
            MessageNameToEventType.TryGetValue(messageType, out var type)
                ? type
                : throw new MessageTypeNotRegisteredException(messageType);

        public static string GetMessageNameForClrType(Type clrType) =>
            MessageNameToEventType.ContainsValue(clrType)
                ? MessageNameToEventType.Single(x => x.Value == clrType).Key
                : throw new MessageTypeNotRegisteredException(clrType.Name);
    }

    public class MessageTypeNotRegisteredException : Exception
    {
        public MessageTypeNotRegisteredException(string typeName) : base($"{typeName} is not registered.")
        {
        }
    }

    public interface IRepository
    {
        Task<Maybe<T>> Load<T>(string id) where T : Aggregate, new();
        Task Save(Aggregate updated);
    }

    public class Maybe<T> where T : class
    {
        public T Value { get; }
        public bool HasValue => Value != null;
        private Maybe(T value)
        {
            Value = value;
        }

        public static Maybe<T> From(T value) => new Maybe<T>(value);
        public static Maybe<T> None => new Maybe<T>(null);
    }
}