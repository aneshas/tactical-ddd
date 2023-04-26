using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Aperture.Core;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using IPullEventStreamEventStore = Aperture.Core.IEventStore;

namespace Tactical.DDD.EventSourcing.Postgres
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(string message) : base(message)
        {
        }
    }

    public class EventStoreConcurrencyCheckException : Exception
    {
        public EventStoreConcurrencyCheckException(string message) : base(message)
        {
        }
    }

    public class EventStore : IEventStore, IPullEventStreamEventStore
    {
        private readonly NpgsqlDataSource _conn;

        private readonly JsonSerializerSettings _jsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Ignore,
            MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
        };

        public EventStore(NpgsqlDataSource conn)
        {
            _conn = conn;
        }

        public async Task<IEnumerable<Event>> LoadEventsAsync(EntityId aggregateId)
        {
            await using var conn = _conn.CreateConnection();

            const string sql = @"SELECT 
                                id Id, 
                                stream_id StreamId, 
                                stream_version StreamVersion, 
                                stream_name StreamName, 
                                data ""Data"", 
                                meta Meta, 
                                created_on CreatedOn 
                                FROM public.events
                                WHERE stream_id::text = @AggregateId
                                ORDER BY id";

            var storedEvents = await conn.QueryAsync<StoredEvent>(
                sql,
                new {AggregateId = aggregateId.ToString()});

            var enumerable = storedEvents as StoredEvent[] ?? storedEvents.ToArray();

            if (storedEvents == null || !enumerable.Any())
            {
                throw new AggregateNotFoundException($"No events found for {aggregateId}");
            }

            return enumerable.Select(x =>
                new Event(
                    x.Id,
                    x.StreamId,
                    x.StreamVersion,
                    x.StreamName,
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(x.Meta),
                    JsonConvert.DeserializeObject(x.Data, _jsonSerializerSettings) as DomainEvent));
        }

        public async Task SaveEventsAsync(
            string aggregateName,
            EntityId aggregateId,
            int expectedVersion,
            IEnumerable<DomainEvent> events,
            Dictionary<string, string> meta)
        {
            await using var conn = _conn.CreateConnection();
            
            const string sql = @"INSERT INTO 
                            events(stream_id, stream_version, stream_name, data, meta)
                            VALUES (@stream_id::uuid, @stream_version, @stream_name, @data::jsonb, @meta::jsonb)";

            var data = events.Select(x => new
            {
                stream_id = aggregateId.ToString(),
                stream_version = ++expectedVersion,
                stream_name = aggregateName,
                data = JsonConvert.SerializeObject(x, _jsonSerializerSettings),
                meta = JsonConvert.SerializeObject(meta)
            });

            try
            {
                await conn.ExecuteAsync(sql, data);
            }
            catch (PostgresException e)
            {
                if (e.Message.Contains("23505"))
                    throw new EventStoreConcurrencyCheckException("Concurrent aggregate update attempted");

                throw;
            }
        }

        public async Task<IEnumerable<EventData>> LoadEventsAsync(Type projection, int fromOffset, int count)
        {
            await using var conn = _conn.CreateConnection();
            
            const string sql = @"SELECT 
                                id Id, 
                                stream_id StreamId, 
                                stream_version StreamVersion, 
                                stream_name StreamName, 
                                data ""Data"", 
                                meta Meta, 
                                created_on CreatedOn 
                                FROM public.events
                                WHERE id > @Offset
                                ORDER BY id
                                LIMIT @Count";

            var storedEvents = await conn.QueryAsync<StoredEvent>(
                sql,
                new
                {
                    Offset = fromOffset,
                    Count = count
                });

            var enumerable = storedEvents as StoredEvent[] ?? storedEvents.ToArray();

            return enumerable.Select(x =>
                new EventData
                {
                    Offset = (int) x.Id,
                    Event = JsonConvert.DeserializeObject(x.Data, _jsonSerializerSettings) as DomainEvent
                });
        }
    }
}