using System.Collections.Generic;

namespace Tactical.DDD
{
    public interface IAggregate<out TIdentity> : IEntity<TIdentity>
        where TIdentity : IEntityId 
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    }
}