using System;

namespace Tactical.DDD
{
    public abstract record DomainEvent
    {
        public DateTime CreatedAt { get; init; }
    }
}