using System;

namespace Tactical.DDD.Tests.TestAggregate
{
    public class SubTaskAdded : IDomainEvent
    {
        public IEntityId AggregateId { get; }

        public DateTime CreatedAt { get; set; }
        
        public string Title { get; }
        
        public TaskId TaskId { get; }

        public SubTaskAdded(TaskId id, string title)
        {
            CreatedAt = DateTime.Now;
            
            TaskId = id;
            Title = title;
        }
    }
}