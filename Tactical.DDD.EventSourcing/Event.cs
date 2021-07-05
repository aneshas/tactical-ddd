using System;
using System.Collections.Generic;

namespace Tactical.DDD.EventSourcing
{
    public record Event(
        uint Id,
        Guid StreamId,
        int StreamVersion,
        string StreamName,
        Dictionary<string, string> Meta,
        DomainEvent DomainEvent);
}