# Tactical DDD Helpers

[![Build Status](https://travis-ci.org/aneshas/tactical-ddd.svg?branch=master)](https://travis-ci.org/aneshas/tactical-ddd)
[![Build status](https://ci.appveyor.com/api/projects/status/vef5ta3j36p7efnn?svg=true)](https://ci.appveyor.com/project/aneshas/tactical-ddd)

`Install-Package TacticalDDD`

TacticalDDD contains lightweight helpers that I find myself implementing over and over again related to DDD/Event
Sourcing tactical patterns, such as Value Objects, Entities, AggregateRoots, EntityIds etc..

These helpers are mostly in the form of simple abstractions that provide help around equality and contain useful helper
methods...

## 5.0.0 Breaking changes

Heads up.
I did a major refactor and there are a lot of breaking changes plus the package now
depends on .NET 5 and uses C# 9.

Version `1.0.32` is the last version that supported C# 7 and .NET Standard

### Reasoning behind this package

I am a big proponent of "Little copy is better than a little dependency" mantra, but I also believe there exists a small
set of facts that should always be true when implementing DDD patterns. Keeping this in mind I did (and will) try to
keep this package as thin and as pragmatic as possible.

Note: I assume that you are well versed in these topics.

#### 1. There is almost always a notion of an Entity

Entity is something that has an Identity (Id), and thus this dictates Entity equality implementation, eg. an entity is
equal to another entity if their Id's are equal. This is exactly what `Entity` abstract class provides: An Id and
equality implementations.

#### 2. What is an id

`EntityId` is something that has to be serializable to string eg. it needs to implement `ToString()` method. I provided
a helper record for you to use, named `EntityId`. It extends `record` instead of a `class` which means that it provides
structural equality (more on value object below).

An example of Entity and an Entity Id:

```c#
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
```

```c#
    public sealed class Customer : Entity<CustomerId>
    {
        
    }
```

This is how you would define an entity along with it's id.

What this tries to enforce is that your entities always have an id and that the id is a value object (avoiding primitive obsession). I will not go into much

details why this is useful.

#### 3. AggregateRoots and Domain Events

Like you may already know aggregate root is an entity that sits on top of an aggregate tree. So in my implementation
this is what it is. `AggregateRoot` extends `Entity` and provides same identity utilities.

One extra thing an `AggregateRoot` has is ability to deal with domain events, eg. it provides a public readonly
collection of domain events and some protected methods to manage that collection.

In previous versions I had a separate `AggregateRoot` implementation that contained utility methods for
an event sourced aggregate. In retrospect I think this was a bit confusing and redundant so in version `5.0.0` I
decided to merge those two.

So for example if you decided that your `Customer` entity is in fact an aggregate, this is how you would implement it as an `AggregateRoot`:

```c#
    public sealed class Customer : AggregateRoot<CustomerId>
    {
        
    }
```

Here is an example of an event sourced `Customer` aggregate:

```c#
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
```

#### 4. Value Objects

Last but not least. Value objects imho are the most valuable pattern.

Prior to `5.0.0` this package contained `ValueObject` abstract class that helped with structural 
equality. With C# 9 this is no longer necessary so I ditched it.

Records pretty much provide you with all that is needed to create your own value objects.
All you need to do is to provide your own constraints, mostly during creation / parsing.

With `ValueObject` being out of the picture, I did add another helper record named `ConstrainedValue` which
provides a neat way of wrapping primitive types and adding simple constraints via built in data annotations in order to avoid primitive obsession.

An example usage of `ConstrainedValue`:

```c#
    // We create a simple wrapper for our string that enforces
    // string length. Second generic parameter enforces the type of exception
    // thrown if validation fails.
    public sealed record String10 : ConstrainedValue<string, DomainException>
    {
        public String10(
            [MinLength(5)]
            [MaxLength(10)] string value) : base(value)
        {
        }
    }

    // Now our Name value object can simply be defined as:
    public sealed record Name(String10 FirstName, String10 LastName);
}
```

```c#
    // Value itself can simply be used as
    var value = new String10("A value");
    
    // And is implicitly assignable to it's generic type (in this case a string)
    string val = value;
``` 

You can and should use `ConstrainedValue` in order to create simple wrappers around primitive types
eg String50, PositiveInt, FutureDate etc... which contain simple constraints and which you then can compose into
more complex value objects (records)
