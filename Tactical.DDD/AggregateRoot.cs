using System.Collections.Generic;

namespace Tactical.DDD
{
    public abstract class AggregateRoot<TIdentity> : Entity<TIdentity>
        where TIdentity : EntityId
    {
        private readonly List<DomainEvent> _domainEvents = new();

        protected AggregateRoot()
        {
        }

        protected void AddDomainEvent(DomainEvent @event) =>
            _domainEvents.Add(@event);

        protected void RemoveDomainEvent(DomainEvent @event) =>
            _domainEvents.Remove(@event);

        protected void ClearDomainEvents() =>
            _domainEvents.Clear();

        public IReadOnlyCollection<DomainEvent> DomainEvents =>
            _domainEvents.AsReadOnly();

        #region EventSourcing

        public int Version { get; private set; }

        public AggregateRoot(IEnumerable<DomainEvent> events)
        {
            if (events == null) return;

            foreach (var domainEvent in events)
            {
                Mutate(domainEvent);
                Version++;
            }
        }

        protected void Apply(IEnumerable<DomainEvent> events)
        {
            foreach (var @event in events)
            {
                Apply(@event);
            }
        }

        protected void Apply(DomainEvent @event)
        {
            Mutate(@event);
            AddDomainEvent(@event);
        }

        private void Mutate(DomainEvent @event) =>
            ((dynamic) this).On((dynamic) @event);
    }

    #endregion
}