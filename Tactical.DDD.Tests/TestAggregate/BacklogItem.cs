using System;
using System.Collections.Generic;

namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed class BacklogItem : AggregateRoot<BacklogItemId>
    {
        public string Summary { get; private set; }

        private BacklogItem()
        {
        }

        public BacklogItem(IReadOnlyCollection<DomainEvent> events) : base(events)
        {
        }

        public static BacklogItem FromSummary(BacklogItemId id, string summary)
        {
            var item = new BacklogItem();

            item.Apply(new BacklogItemCreated
            (
                DateTime.UtcNow,
                id,
                summary
            ));

            return item;
        }

        public SubTaskId AddTask(string title)
        {
            var task = new SubTask(new SubTaskId(), title);

            AddDomainEvent(new SubTaskAdded
            (
                DateTime.UtcNow,
                Id,
                task.Id,
                title
            ));

            return task.Id;
        }

        public void On(BacklogItemCreated @event)
        {
            Id = BacklogItemId.Parse(@event.BacklogItemId);
            Summary = @event.Summary;
        }
    }
}