using System;

namespace Tactical.DDD
{
    public interface IDomainEvent
    {
        IEntityId AggregateId { get; }

        DateTime CreatedAt { get; }
    }
}