using System.Collections.Generic;

namespace Tactical.DDD.EventSourcing
{
    public class AggregateRoot<TIdentity> : DDD.AggregateRoot<TIdentity>, IAggregate<TIdentity>
        where TIdentity : IDomainIdentity
    {
        private readonly List<IDomainEvent> _uncommittedEvents = new List<IDomainEvent>();

        public int Version { get; private set; }

        public IEnumerable<IDomainEvent> UncommittedEvents => _uncommittedEvents; 

        protected AggregateRoot(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                Mutate(domainEvent);
            }
        }

        private void Mutate(IDomainEvent @event)
        {
            ((dynamic) this).When((dynamic) @event);
            Version++;
        } 

        protected void Apply(IDomainEvent @event)
        {
            Mutate(@event);
            _uncommittedEvents.Add(@event);
        }
    }
}