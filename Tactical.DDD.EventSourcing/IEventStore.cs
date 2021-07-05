using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tactical.DDD.EventSourcing
{
    public interface IEventStore
    {
        Task<IEnumerable<Event>> LoadEventsAsync(EntityId aggregateId);

        Task SaveEventsAsync(
            string aggregateName, 
            EntityId aggregateId, 
            int expectedVersion,
            IEnumerable<DomainEvent> events,
            Dictionary<string, string> meta);
    }
}