using Xunit;

namespace Tactical.DDD.Tests
{
    public class ConstrainedValueTests
    {
        [Fact]
        public void ExceptionIsThrownForAnInvalidValue()
        {
            var str = "asdfgkljhhasdfgkljhhasdfgkljhhasdfgkljhahasdfgkljhh";

            Assert.Throws<DomainException>(() => new String50(str));
        }

        [Theory]
        [InlineData("min")]
        [InlineData("maximum exceeded")]
        public void ExceptionIsThrownForMultipleValidations(string str)
        {
            Assert.Throws<DomainException>(() => new String10(str));
        }

        [Fact]
        public void ValueConstructedAfterValidations()
        {
            var str = "valid";

            var value = new String10(str);

            Assert.Equal(str, value);
        }

        [Fact]
        public void ConstrainedValueIsImplicitlyConvertibleToGenericType()
        {
            var str = new String50("A value");
            string value = str;
        }
    }
}