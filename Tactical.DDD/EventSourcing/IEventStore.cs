using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tactical.DDD.EventSourcing
{
    public interface IEventStore
    {
        Task<IEnumerable<IDomainEvent>> LoadEventsAsync(IEntityId aggregateId);

        Task SaveEventsAsync(IEntityId aggregateId, int expectedVersion, IEnumerable<IDomainEvent> events);
    }
} 