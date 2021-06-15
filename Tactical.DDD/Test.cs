using System;
using System.Collections.Generic;

namespace Tactical.DDD
{
    public sealed record CustomerId : EntityId
    {
        private Guid _guid;

        private CustomerId(string guid) =>
            Guid.Parse(guid);

        public CustomerId() =>
            _guid = Guid.NewGuid();

        // You might implement this static factory method in order to be able to
        // parse your id from string
        public static CustomerId Parse(string id) => new(id);

        // ToString implementation
        public override string ToString() => _guid.ToString();
    }


    ////
    ///
    ///
    public sealed class Customer : AggregateRoot<CustomerId>
    {
        // We re-export constructor provided by our AggregateRoot implementation
        // which is used to rehydrate our aggregate from domain events
        public Customer(IEnumerable<DomainEvent> events) : base(events)
        {
        }

        // We want to encapsulate and thus hide parameterless constructor 
        private Customer()
        {
        }

        // Use case method
        // (newName would preferably be value object in itself instead of a primitive type
        public void ChangeNameTo(string newName)
        {
            Apply(
                new CustomerChangedName
                {
                    CreatedAt = DateTime.UtcNow, // Don't do this
                    CustomerId = Id, // Notice how CustomerId is implicitly convertible to string
                    NewName = newName
                }
            );
        }

        // This method is automagically called when we Apply a domain event and also
        // if we instantiate our aggregate via `public Customer(IEnumerable<IDomainEvent> events)`
        public void On(CustomerChangedName @event)
        {
            // ... implement the actual mutation 
        }
    }

    // Our domain event
    public sealed record CustomerChangedName : DomainEvent
    {
        public string CustomerId { get; init; }

        public string NewName { get; init; }
    }

    public record String50 : ConstrainedValue<string, ArgumentException>
    {
        private static bool Rule(string value) => value.Length < 50;

        public String50(string value) : base(value, Rule)
        {
        }
    }

    public sealed record Name(String50 FirstName, String50 LastName);
}