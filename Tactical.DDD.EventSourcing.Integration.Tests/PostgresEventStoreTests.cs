using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Npgsql;
using Tactical.DDD.EventSourcing.Postgres;
using Xunit;

namespace Tactical.DDD.EventSourcing.Integration.Tests
{
    public class PostgresEventStoreTests : IntegrationTest
    {
        [Fact]
        public async Task ShouldLoadSavedEventsPerAggregate()
        {
            var aggregateAId = new FooId();
            var aggregateAEvents = new List<DomainEvent>
            {
                new FooEvent(1, DateTime.Now),
                new FooEvent(2, DateTime.Now),
                new FooEvent(3, DateTime.Now)
            };

            var aggregateBId = new FooId();
            var aggregateBEvents = new List<DomainEvent>
            {
                new FooEvent(4, DateTime.Now),
                new FooEvent(5, DateTime.Now),
                new FooEvent(6, DateTime.Now)
            };

            var meta = new Dictionary<string, string>
            {
                {"foo", "bar"},
                {"client-ip", "127.0.0.1"}
            };

            await EventStore.SaveEventsAsync(
                nameof(FooAggregate), aggregateAId, 0, aggregateAEvents, meta);

            await EventStore.SaveEventsAsync(
                nameof(FooAggregate), aggregateBId, 0, aggregateBEvents, meta);

            var events = await EventStore.LoadEventsAsync(aggregateAId);

            Action<Event> AssertEvent(int version, int value) =>
                @event =>
                {
                    @event.Id.Should().Be((uint) value);
                    @event.StreamId.Should().Be(aggregateAId);
                    @event.StreamVersion.Should().Be(version);
                    @event.StreamName.Should().Be(nameof(FooAggregate));
                    @event.Meta.Should().BeEquivalentTo(meta);

                    var domainEvent = @event.DomainEvent as FooEvent;

                    Assert.NotNull(domainEvent);

                    domainEvent.Value.Should().Be(value);
                };

            events
                .Should()
                .SatisfyRespectively(
                    AssertEvent(1, 1),
                    AssertEvent(2, 2),
                    AssertEvent(3, 3)
                );
        }

        [Fact]
        public async Task ShouldThrowNotFoundException() =>
            await Assert.ThrowsAsync<AggregateNotFoundException>(() =>
                EventStore.LoadEventsAsync(new FooId()));

        [Fact]
        public async Task ShouldThrowConcurrencyCheckException()
        {
            var fooId = new FooId();
            var events = new List<DomainEvent>
            {
                new FooEvent(1, DateTime.Now),
                new FooEvent(2, DateTime.Now),
                new FooEvent(3, DateTime.Now)
            };

            await EventStore.SaveEventsAsync(
                nameof(FooAggregate), fooId, 0, events, new Dictionary<string, string>());

            await Assert.ThrowsAsync<EventStoreConcurrencyCheckException>(() =>
                EventStore.SaveEventsAsync(
                    nameof(FooAggregate), fooId, 0, events, new Dictionary<string, string>()));
        }

        [Fact]
        public async Task ShouldLoadEventsFromOffset()
        {
            var fooId = new FooId();
            var events = new List<DomainEvent>
            {
                new FooEvent(1, DateTime.Now),
                new FooEvent(2, DateTime.Now),
                new FooEvent(3, DateTime.Now),
                new FooEvent(4, DateTime.Now),
                new FooEvent(5, DateTime.Now)
            };

            await EventStore.SaveEventsAsync(
                nameof(FooAggregate), fooId, 0, events, new Dictionary<string, string>());

            var loadedEvents = await EventStore.LoadEventsAsync(typeof(FooAggregate), 2, 2);

            loadedEvents
                .Should()
                .SatisfyRespectively(
                    @event =>
                    {
                        @event.Offset.Should().Be(3);

                        var e = @event.Event as FooEvent;

                        e.Value.Should().Be(3);
                        e.CreatedAt.Should().BeSameDateAs(DateTime.Today);
                    },
                    @event =>
                    {
                        @event.Offset.Should().Be(4);

                        var e = @event.Event as FooEvent;

                        e.Value.Should().Be(4);
                        e.CreatedAt.Should().BeSameDateAs(DateTime.Today);
                    }
                );
        }

        [Fact]
        public async Task NoResultsShouldYieldAnEmptyEnumerable()
        {
            var events = await EventStore.LoadEventsAsync(typeof(FooAggregate), 0, 100);

            events
                .Should()
                .BeEmpty();
        }
    }
}