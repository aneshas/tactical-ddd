using System.ComponentModel.DataAnnotations;

namespace Tactical.DDD.Tests
{
    public sealed record String10 : ConstrainedValue<string, DomainException>
    {
        public String10(
            [MinLength(5)] [MaxLength(10)] string value) : base(value)
        {
        }
    }
}