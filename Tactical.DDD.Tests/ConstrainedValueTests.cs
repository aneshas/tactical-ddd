using System;
using Xunit;

namespace Tactical.DDD.Tests
{
    public class ConstrainedValueTests
    {
        [Fact]
        public void ArgumentExceptionIsThrownForAnInvalidValue()
        {
            var str = "asdfgkljhhasdfgkljhhasdfgkljhhasdfgkljhahasdfgkljhh";

            Assert.Throws<ArgumentException>(() => new String50(str));
        }

        [Fact]
        public void ConstrainedValueIsImplicitlyConvertibleToGenericType()
        {
            var str = new String50("A value");
            string value = str;
        }
    }
}