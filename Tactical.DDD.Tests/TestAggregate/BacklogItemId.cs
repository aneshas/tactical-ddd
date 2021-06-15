using System;

namespace Tactical.DDD.Tests.TestAggregate
{
    public record BacklogItemId : EntityId
    {
        public readonly Guid Guid;

        public BacklogItemId()
        {
            Guid = Guid.NewGuid();
        }

        public BacklogItemId(Guid guid)
        {
            Guid = guid;
        }

        public override string ToString() => Guid.ToString();

        public static BacklogItemId Parse(string id) => new BacklogItemId(Guid.Parse(id));
    }
}