using System.Collections.Generic;
using DDD = Tactical.DDD;

namespace Tactical.DDD.EventSourcing
{
    public interface IAggregate<out TIdentity> : DDD.IAggregate<TIdentity>
        where TIdentity : IDomainIdentity
    {
        int Version { get; }
    }
}