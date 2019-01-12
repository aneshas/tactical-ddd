using System.Collections.Generic;

namespace Tactical.DDD.EventSourcing
{
    public abstract class AggregateRoot<TIdentity> : DDD.AggregateRoot<TIdentity>, IAggregate<TIdentity>
        where TIdentity : IDomainIdentity
    {
        public int Version { get; private set; }

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
            AddDomainEvent(@event);
        }
    }
}