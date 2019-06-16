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

        [Fact]
        public void ValueObject_StructurallyEqualsItsCopy()
        {
            var assignee0 = new Assignee("Max Mathew", "max");
            var assignee1 = assignee0.GetCopy() as Assignee;
            
            Assert.Equal(assignee0.Name, assignee1?.Name);
            Assert.Equal(assignee0.DisplayName, assignee1?.DisplayName);

            Assert.True(assignee0.Equals(assignee1));
            Assert.Equal(assignee0, assignee1);
            Assert.True(assignee0 == assignee1);
            Assert.False(assignee0 != assignee1);
        }
   }
}