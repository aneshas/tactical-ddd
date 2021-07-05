using System;

namespace Tactical.DDD.EventSourcing.Postgres
{
    public sealed record StoredEvent
    {
        public uint Id { get; init; }
        public Guid StreamId { get; init; }
        public int StreamVersion { get; init; }
        public string StreamName { get; init; }
        public string Data { get; init; }
        public string Meta { get; init; }
        public DateTime CreatedOn { get; init; }
    }
}