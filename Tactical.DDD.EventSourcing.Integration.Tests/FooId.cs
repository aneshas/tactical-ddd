using System;

namespace Tactical.DDD.EventSourcing.Integration.Tests
{
    public record FooId : EntityId
    {
        private Guid _guid = Guid.NewGuid();
        public override string ToString() => _guid.ToString();
    }
}