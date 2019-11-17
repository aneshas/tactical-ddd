using System.Collections.Generic;

namespace Tactical.DDD.EventSourcing
{
    public abstract class AggregateRoot<TIdentity> : DDD.AggregateRoot<TIdentity>, IAggregateRoot<TIdentity>
        where TIdentity : IEntityId
    {
        public int Version { get; private set; }

        protected AggregateRoot()
        {
        }

        public AggregateRoot(IEnumerable<IDomainEvent> events)
        {
            if (events == null) return;

            foreach (var domainEvent in events)
            {
                Mutate(domainEvent);
                Version++;
            }
        }
        
        protected void Apply(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                Apply(@event); 
            }
        }

        protected void Apply(IDomainEvent @event)
        {
            Mutate(@event);
            AddDomainEvent(@event);
        }

        private void Mutate(IDomainEvent @event) =>
            ((dynamic) this).On((dynamic) @event);
    }
}