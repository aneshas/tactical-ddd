using System.Collections.Generic;

namespace Tactical.DDD
{
    public interface IAggregate<out TIdentity> : IEntity<TIdentity>
        where TIdentity : IDomainIdentity 
    {
        IReadOnlyCollection<IDomainEvent> UncommittedEvents { get; }
    }
}