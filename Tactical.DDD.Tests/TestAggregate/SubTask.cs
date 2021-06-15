using System.Security.Cryptography;

namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed class SubTask : Entity<SubTaskId>
    {
        public string Title { get; private set; }
        
        public SubTask(SubTaskId id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}