using System.Collections.Generic;

namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed class BacklogItem : AggregateRoot<BacklogItemId>
    {
        public string Summary { get; private set; }
        
        // A Set instead of a List
        public List<Task> Tasks { get; }

        public BacklogItem(BacklogItemId id, string summary)
        {
            Id = id;
            Summary = summary;
        }
    }
}