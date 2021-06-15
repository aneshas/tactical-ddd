using System;

namespace Tactical.DDD.Tests.TestAggregate
{
    public record SubTaskAdded : DomainEvent
    {
        public string BacklogItemId { get; init; }
        
        public string SubTaskId { get; init; }

        public string Title { get; init; }
    }
}