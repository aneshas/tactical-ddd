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
            {
                BacklogItemId = id,
                CreatedAt = DateTime.UtcNow,
                Summary = summary
            });

            return item;
        }

        public SubTaskId AddTask(string title)
        {
            var task = new SubTask(new SubTaskId(), title);

            AddDomainEvent(new SubTaskAdded
            {
                BacklogItemId = Id,
                SubTaskId = task.Id,
                Title = title
            });

            return task.Id;
        }

        public void On(BacklogItemCreated @event)
        {
            Id = BacklogItemId.Parse(@event.BacklogItemId);
            Summary = @event.Summary;
        }
    }
}