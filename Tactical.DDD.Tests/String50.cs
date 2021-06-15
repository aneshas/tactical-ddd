using System;

namespace Tactical.DDD.Tests
{
    public record String50 : ConstrainedValue<string, ArgumentException>
    {
        private static bool Rule(string value) =>
            value != null && value.Length < 50;

        public String50(string value) : base(value, Rule)
        {
        }
    }
}