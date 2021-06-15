using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tactical.DDD
{
    public interface IEventStore
    {
        Task<IEnumerable<DomainEvent>> LoadEventsAsync(EntityId aggregateId);

        Task SaveEventsAsync(EntityId aggregateId, int expectedVersion, IEnumerable<DomainEvent> events);
    }
}