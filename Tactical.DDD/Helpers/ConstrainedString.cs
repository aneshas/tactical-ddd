using System;

namespace Tactical.DDD.Helpers
{
    public record ConstrainedString : ConstrainedValue<string, DomainException>
    {
        private static Func<string, (bool, string)> Rule(int minLength, int maxLength) =>
            value =>
                (value != null && value.Length > minLength && value.Length < maxLength,
                    $"Value should be between {minLength} and {maxLength}.");

        protected ConstrainedString(string value, int minLength, int maxLength)
            : base(value, Rule(minLength, maxLength))
        {
        }
    }
}