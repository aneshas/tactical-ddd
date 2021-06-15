namespace Tactical.DDD.Tests.TestAggregate
{
    public record SubTaskId : EntityId
    {
        public string Id { get; } = "task_id";

        public override string ToString() => Id;
    }
}