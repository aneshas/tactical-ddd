using System;
using Tactical.DDD.Tests.TestAggregate;
using Xunit;

namespace Tactical.DDD.Tests
{
    public class EntityIDentityTests
    {
        [Fact]
        public void TestBacklogItemId()
        {
            var guid = Guid.NewGuid();
            var id = new BacklogItemId(guid);

            var item = new BacklogItem(id, "item summary");

            Assert.Equal(guid.ToString(), item.Id.Identity);
        }

        [Fact]
        public void TestEntityIdentityEquality()
        {
            var guid = Guid.NewGuid();

            var id0 = new BacklogItemId(guid);
            var id1 = new BacklogItemId(guid);

            Assert.True(id0.Equals(id1));
            Assert.Equal(id0, id1);
            Assert.True(id0 == id1);
            Assert.False(id0 != id1);
        }
    }
}