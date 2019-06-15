using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tactical.DDD.EventSourcing
{
    public interface IEventStore
    {
        Task<IReadOnlyCollection<IDomainEvent>> LoadEventsAsync(IEntityId aggregateId);

        Task SaveEventsAsync(IEntityId aggregateId, int version, IReadOnlyCollection<IDomainEvent> events);
    }
} 