using System.Collections.Generic;

namespace Tactical.DDD
{
    public interface IAggregateRoot<out TIdentity> : IEntity<TIdentity>
        where TIdentity : IEntityId 
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    }
}