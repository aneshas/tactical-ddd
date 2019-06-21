using System.Collections.Generic;
using DDD = Tactical.DDD;

namespace Tactical.DDD.EventSourcing
{
    public interface IAggregateRoot<out TIdentity> : DDD.IAggregateRoot<TIdentity>
        where TIdentity : IEntityId
    {
        int Version { get; }
    }
}