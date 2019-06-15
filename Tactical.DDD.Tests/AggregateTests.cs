using System;
using System.Linq;
using Tactical.DDD.Tests.TestAggregate;
using Xunit;

namespace Tactical.DDD.Tests
{
    public class AggregateTests
    {
        [Fact]
        public void EntitiesWithSameIdentityAndProps_ShouldBeEqual()
        {
            var guid = Guid.NewGuid();

            var id = new BacklogItemId(guid);

            var item0 = BacklogItem.FromSummary(id, "item summary");
            var item1 = BacklogItem.FromSummary(id, "item summary");

            Assert.True(item0.Equals(item1));
            Assert.Equal(item0, item1);
            Assert.True(item0 == item1);
        }

        [Fact]
        public void EntitiesWithSameIdentityAndDifferentProps_ShouldBeEqual()
        {
            var guid = Guid.NewGuid();

            var id = new BacklogItemId(guid);

            var item0 = BacklogItem.FromSummary(id, "item summary");
            var item1 = BacklogItem.FromSummary(id, "other item summary");

            Assert.True(item0.Equals(item1));
            Assert.Equal(item0, item1);
            Assert.True(item0 == item1);
            Assert.False(item0 != item1);
        }

        [Fact]
        public void EntitiesWithDifferentIdentityAndSameProps_ShouldNotBeEqual()
        {
            var item0 = BacklogItem.FromSummary(new BacklogItemId(), "item summary");
            var item1 = BacklogItem.FromSummary(new BacklogItemId(), "item summary");

            Assert.False(item0.Equals(item1));
            Assert.NotEqual(item0, item1);
            Assert.False(item0 == item1);
            Assert.True(item0 != item1);
        }

        [Fact]
        public void ESAggregateA_Applies_Domain_Events()
        {
            var id = new BacklogItemId();
            
            var item = BacklogItem.FromSummary(id, "summary");
            
            Assert.Equal(id, item.Id);
            Assert.Equal("summary", item.Summary);
            Assert.Equal(1, item.Version);
            
            Assert.Contains(item.DomainEvents, e => e is BacklogItemCreated);
        }
    }
}