using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Tactical.DDD.EventSourcing.Integration.Tests
{
    public class PostgresOffsetTrackerTests : IntegrationTest
    {
        [Fact]
        public async Task InitialOffsetQueryShouldReturnZeroOffset()
        {
            var offset = await OffsetTracker.GetOffsetAsync(GetType());

            offset.Should().Be(0);
        }

        [Fact]
        public async Task ShouldLoadSavedOffset()
        {
            var offset = 100;

            await OffsetTracker.GetOffsetAsync(GetType());
            await OffsetTracker.SaveOffsetAsync(GetType(), 10);
            await OffsetTracker.SaveOffsetAsync(GetType(), 50);
            await OffsetTracker.SaveOffsetAsync(GetType(), offset);

            var gotOffset = await OffsetTracker.GetOffsetAsync(GetType());

            gotOffset.Should().Be(offset);
        }
    }
}