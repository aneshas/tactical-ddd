using System;

namespace Tactical.DDD
{
    public interface IDomainEvent
    {
        DateTime CreatedAt { get; }
    }
}