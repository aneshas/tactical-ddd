using System;
using System.Collections.Generic;

namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed class BacklogItem : EventSourcing.AggregateRoot<BacklogItemId>
    {
        public string Summary { get; private set; }

        private BacklogItem() : base()
        {
        }

        public BacklogItem(IEnumerable<IDomainEvent> events) : base(events)
        {
        }

        public static BacklogItem FromSummary(BacklogItemId id, string summary)
        {
            var item = new BacklogItem();

            item.Apply(new BacklogItemCreated(summary, id.ToString()));

            return item;
        }

        public TaskId AddTask(string title)
        {
            var task = new SubTask(new TaskId(), title);
            AddDomainEvent(new SubTaskAdded(task.Id, task.Title));

            return task.Id;
        }

        public void On(BacklogItemCreated @event)
        {
            Id = new BacklogItemId(Guid.Parse(@event.BacklogItemId));
            Summary = @event.Summary;
        }
    }
}