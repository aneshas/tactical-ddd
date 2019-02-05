using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tactical.DDD.EventSourcing
{
    public interface IEventStore
    {
        Task<IEnumerable<IDomainEvent>> LoadEventsAsync(IDomainIdentity aggregateId);

        Task SaveEventsAsync(IDomainIdentity aggregateId, int version, IReadOnlyCollection<IDomainEvent> events);
    }
} 