using System;

namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed class BacklogItemCreated : IDomainEvent
    {
        public string Summary { get; }

        public BacklogItemId BacklogItemId { get; }

        public IEntityId AggregateId { get; }

        public DateTime CreatedAt { get; set; }
        
        public BacklogItemCreated(string summary, BacklogItemId backlogItemId)
        {
            CreatedAt = DateTime.Now;
            
            Summary = summary;
            BacklogItemId = backlogItemId;
        }
    }
}