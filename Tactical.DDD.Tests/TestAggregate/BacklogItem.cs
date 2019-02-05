using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed class BacklogItem : AggregateRoot<BacklogItemId>
    {
        public string Summary { get; private set; }
        
        public List<SubTask> Tasks { get; }

        public BacklogItem(BacklogItemId id, string summary)
        {
            Id = id;
            Summary = summary;
            
            Tasks = new List<SubTask>();
        }

        public TaskId AddTask(string title)
        {
            var task = new SubTask(new TaskId(), title);
            AddDomainEvent(new SubTaskAdded(task.Id, task.Title));

            return task.Id;
        }
    }
}