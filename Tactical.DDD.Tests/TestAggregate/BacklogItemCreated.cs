using System;

namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed record BacklogItemCreated : DomainEvent
    {
        public string BacklogItemId { get; init; }
        
        public string Summary { get; init; }
    }
}