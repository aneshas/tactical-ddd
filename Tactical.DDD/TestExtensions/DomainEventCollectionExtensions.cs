using System;
using System.Collections.Generic;
using System.Linq;

namespace Tactical.DDD.TestExtensions
{
    public static class DomainEventCollectionExtensions
    {
        public static void Expect<T>(this IEnumerable<IDomainEvent> events, Action<T> assertAction)
            where T : class
        {
            var tEvents = events.Where(e => e is T).ToList();

            if (tEvents == null) throw new InvalidOperationException($"No events of type {typeof(T)} found.");

            var exceptions = new List<Exception>();

            foreach (var @event in tEvents)
            {
                try
                {
                    assertAction(@event as T);
                    return;
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            throw new AggregateException(exceptions);
        }

        public static void ExpectOne<T>(this IEnumerable<IDomainEvent> events, Action<T> assertAction)
            where T : class
        {
            var @event = events.Single(e => e is T);
            assertAction(@event as T);
        }
    }
}
