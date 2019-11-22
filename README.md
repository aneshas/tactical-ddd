# Tactical DDD Helpers
[![Build Status](https://travis-ci.org/aneshas/tactical-ddd.svg?branch=master)](https://travis-ci.org/aneshas/tactical-ddd)
[![Build status](https://ci.appveyor.com/api/projects/status/vef5ta3j36p7efnn?svg=true)](https://ci.appveyor.com/project/aneshas/tactical-ddd)

This repository contains lightweight helpers to help implement common DDD/Event Sourcing tactical patterns, such as ValueObject,
Entity, AggregateRoot-s, EntityId-s etc..
These helpers mostly provide help around equality and by providing some useful methods to deal with domain events...

### Reasoning behind this package
The reason this package exists is because there exists a small set of facts that should always be true when implementing DDD patterns.
I will not go too much into details, I will assume that you are well versed in these topics.

#### 1. There is almost always a notion of an Entity
Entity is something that has an Identity (Id), and thus this dictates Entity equality implementation, eg. an entity is equal to another
entity if their Id's are equal. This is exactly what `Entity` abstract class provides: An Id and equality implementations.

`Entity` implements `IEntity` which ensures that you need to have an Id

#### 2. What is an id
I chose that an `EntityId` is something that has to be serializable to string eg. it needs to implement `ToString()` method.
I provided a helper class for you to use which is named `EntityId`. Apart from implementing `IEntityId` it is also marked 
as a `ValueObject` which means that it provides structural equality (more on value object below).

An example of Entity and an Entity Id:
```c#
using Tactical.DDD;

public sealed class PersonId : EntityId {
  private Guid _guid;
  
  public PersonId() {
    _guid = Guid.NewGuid();
  }
  
  // You might implement this constructor in order to be able to
  // parse your id from string
  public PersonId(string id) {
    _guid = Guid.Parse(id);
  }
  
  // Mandatory ToString implementation
  public override string ToString() => _guid.ToString();
}
```
```c#
using Tactical.DDD;

public sealed class Person : Entity<PersonId> {
  public override PersonId Id { get; protected set; }
}
```
So this is how you would define an entity along with it's id.

What this tries to enforce is that your Id's are value objects but not some primitive values. I will not go into much
details why avoiding "primitive obsession" is useful.

#### 3. AggregateRoots and Domain Events
Like you may already know aggregate root is an entity that sits on top of an aggregate tree.
So in my implementation this is what it is. `AggregateRoot` extends `Entity` and provides same identity utilities.

One extra thing an `AggregateRoot` has is ability to deal with domain events, eg. it provides a public readonly collection of
domain events and some protected methods to manage that collection.

So for example if you decided that your `Person` entity is in fact an aggregate, this is how you would implement it:

```c#
using Tactical.DDD;

public sealed class Person : AggregateRoot<PersonId> {
  public override PersonId Id { get; protected set; }
}
```

There is another implementation of `AggregateRoot` in `Tactical.DDD.EventSourcing` namespace that provides extra capabilities
if you are modeling an event sourced aggregate, namely `Apply(IDomainEvent @event)` method and a few constructors.

Here is an example of an event sourced `Person` aggregate:
```c#
using Tactical.DDD.EventSourcing;

public sealed class Person : AggregateRoot<PersonId> {
  public override PersonId Id { get; protected set; }
  
  // We re-export constructor provided by our AggregateRoot implementation
  // which is used to rehydrate our aggregate from domain events
  public Person(IEnumerable<IDomainEvent> events) : base(events) { }
  
  // We want to encapsulate and thus hide parameterless constructor 
  private Person() { }
  
  // A use case method
  public void ChangeName(string name) {
    // ... validate
    
    // Apply domain event
    Apply(new PersonChangedName(name));
  }
  
  // This method is automagically called when we Apply PersonChangedName and also
  // if we instantiate our aggregate via `public Person(IEnumerable<IDomainEvent> events)`
  public void On(PersonChangedName @event) {
    // ... do stuff, eg. mutate the aggregate itself
  }
}

// Our domain event
public sealed class PersonChangedName : IDomainEvent {
  public DateTime CreatedAt { get; set; } // required by IDomainEvent
  
  public string NewName { get; }
  
  public PersonChangedName(string newName) {
    CreatedAt = DateTime.Now();
    NewName = newName
  }
}
```

#### 4. Value Objects
Again, won't go into details why value objects are useful.
What this package provides is an implementation of equality methods in order to provide structural equality.

Example of an immutable structurally equal value object:
```c#
public sealed class Address : ValueObject {
  public string Street { get; }
  
  public string HouseNumber { get; }
  
  public Address(string street, string houseNumber) {
    Street = street;
    HouseNumber = houseNumber;
  }
  
  // We are required to implement this method in order to provide
  // the properties that we want to participate in structural equality
  // eg. in this case an address is equal to another address if both
  // Street and HouseNumber properties are equal
  protected override IEnumerable<object> GetAtomicValues()
  {
    yield return Street;
    yield return HouseNumber;
  }
}
```

There are a few other things which I will explain on another occasion.

