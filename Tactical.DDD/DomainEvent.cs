using System;

namespace Tactical.DDD
{
    public abstract record DomainEvent(DateTime CreatedAt);
}