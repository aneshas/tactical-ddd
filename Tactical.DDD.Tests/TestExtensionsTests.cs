using Tactical.DDD.Testing;
using Tactical.DDD.Tests.TestAggregate;
using Xunit;

namespace Tactical.DDD.Tests
{
    public class TestExtensionsTests
    {
        [Fact]
        public void ExpectOneExtension_Passes_If_Exactly_One_Matching_Event()
        {
            var id = new BacklogItemId();

            var item = BacklogItem.FromSummary(id, "summary");

            item.DomainEvents.ExpectOne<BacklogItemCreated>(e =>
            {
                Assert.Equal(id, e.BacklogItemId);
                Assert.Equal("summary", e.Summary);
            });
        }

//        [Fact]
//        public void ExpectOneExtension_Fails_If_Exactly_One_Not_Matching_Event()
//        {
//            var id = new BacklogItemId();
//
//            var item = BacklogItem.FromSummary(id, "summary");
//
//            item.DomainEvents.ExpectOne<BacklogItemCreated>(e => { Assert.Equal("test", e.Summary); });
//        }
//
//        [Fact]
//        public void ExpectOneExtension_Fails_If_Multiple_Events()
//        {
//            var id = new BacklogItemId();
//
//            var item = BacklogItem.FromSummary(id, "summary");
//
//            // ProducedEvents ???
//            item.DomainEvents.ExpectOne<BacklogItemCreated>(e => { Assert.Equal("test", e.Summary); });
//        }
    }
}