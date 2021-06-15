using System;

namespace Tactical.DDD
{
    public class DomainException : Exception
    {
        public DomainException()
        {
        }

        protected DomainException(string message) : base(message)
        {
        }

        protected DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}