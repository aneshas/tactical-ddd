using System;

namespace Tactical.DDD.EventSourcing.Integration.Tests
{
    public record FooEvent(int Value, DateTime CreatedAt) : DomainEvent(CreatedAt);
}