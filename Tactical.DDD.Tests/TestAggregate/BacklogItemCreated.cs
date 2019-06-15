using System;

namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed class BacklogItemCreated : IDomainEvent
    {
        public string Summary { get; }

        public string BacklogItemId { get; }

        public DateTime CreatedAt { get; }

        public BacklogItemCreated(string summary, string backlogItemId)
        {
            CreatedAt = DateTime.Now;
            
            Summary = summary;
            BacklogItemId = backlogItemId;
        }
    }
}