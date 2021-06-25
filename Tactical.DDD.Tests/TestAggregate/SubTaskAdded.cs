using System;

namespace Tactical.DDD.Tests.TestAggregate
{
    public record SubTaskAdded
    (
        DateTime CreatedAt,
        string BacklogItemId,
        string SubTaskId,
        string Title
    ) : DomainEvent(CreatedAt);
}