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

            var item = BacklogItem.FromSummary(id, "item summary");

            Assert.Equal(guid.ToString(), item.Id.ToString());
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

        [Fact]
        public void IDomainIdentity_ToString_Calls_ToString_Override()
        {
            var guid = Guid.NewGuid();
            
            var id = new BacklogItemId(guid);
            
            Assert.Equal(guid.ToString(), ((IEntityId) id).ToString());
        }
    }
}