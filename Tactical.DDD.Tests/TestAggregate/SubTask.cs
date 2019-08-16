using System.Security.Cryptography;

namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed class SubTask : Entity<TaskId>
    {
        public override TaskId Id { get; protected set; }

        public string Title { get; private set; }
        
        public SubTask(TaskId id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}