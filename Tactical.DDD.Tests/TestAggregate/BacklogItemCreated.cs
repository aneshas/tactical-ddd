using System;

namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed record BacklogItemCreated
    (
        DateTime CreatedAt,
        string BacklogItemId,
        string Summary
    ) : DomainEvent(CreatedAt);
}