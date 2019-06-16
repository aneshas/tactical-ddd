using System.Collections.Generic;

namespace Tactical.DDD.EventSourcing
{
    public abstract class AggregateRoot<TIdentity> : DDD.AggregateRoot<TIdentity>, IAggregate<TIdentity>
        where TIdentity : IEntityId
    {
        public int Version { get; private set; }

        protected AggregateRoot() {}
        
        protected AggregateRoot(IReadOnlyCollection<IDomainEvent> events)
        {
            if (events == null) return;
            
            foreach (var domainEvent in events)
            {
                Mutate(domainEvent);
            }
        }

        private void Mutate(IDomainEvent @event)
        {
            ((dynamic) this).On((dynamic) @event);
            Version++;
        } 

        protected void Apply(IDomainEvent @event)
        {
            Mutate(@event);
            AddDomainEvent(@event);
        }
    }
}