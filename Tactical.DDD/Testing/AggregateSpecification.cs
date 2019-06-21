using System;
using System.Collections.Generic;

namespace Tactical.DDD.Testing
{
    public abstract class AggregateSpecification<T, TId>
        where T : IAggregateRoot<TId> where TId: IEntityId 
    {
        protected T Aggregate;
        protected IReadOnlyCollection<IDomainEvent> ProducedEvents = default;
        protected Exception ExceptionThrown = null;

        protected abstract T Given();

        protected abstract void When();

        /// <summary>
        /// Constructor will execute specification.
        /// Warning: Given and When will be executed before the derived types constructor!
        /// Use Given method to initialize your derived specification instead of constructor!
        /// </summary>
        protected AggregateSpecification()
        {
            //
            Aggregate = Given();

            try
            {
                When();
                ProducedEvents = Aggregate.DomainEvents;
            }
            catch (Exception ex)
            {
                ExceptionThrown = ex;
            }
        }
    }
}