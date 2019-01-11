using Tactical.DDD.Tests.TestAggregate;
using Xunit;

namespace Tactical.DDD.Tests
{
    public class ValueObjectTests
    {
        [Fact]
        public void ValueObject_StructurallyEqualsAnotherValueObject()
        {
            var assignee0 = new Assignee("John Doe", "john");
            var assignee1 = new Assignee("John Doe", "john");
            
            Assert.True(assignee0.Equals(assignee1));
            Assert.Equal(assignee0, assignee1);
            Assert.True(assignee0 == assignee1);
            Assert.False(assignee0 != assignee1);
        }
        
        [Fact]
        public void ValueObject_StructurallyNotEqualsAnotherValueObject()
        {
            var assignee0 = new Assignee("Max Mathew", "max");
            var assignee1 = new Assignee("John Doe", "john");
            
            Assert.False(assignee0.Equals(assignee1));
            Assert.NotEqual(assignee0, assignee1);
            Assert.False(assignee0 == assignee1);
            Assert.True(assignee0 != assignee1);
        }
        
        // TODO - Test pvt / protected fields ...
    }
}