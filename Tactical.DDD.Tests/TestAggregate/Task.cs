namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed class Task : Entity<TaskId>
    {
        public Task(TaskId id)
        {
            Id = id;
        }
    }
}