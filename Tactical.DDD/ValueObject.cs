using System.Collections.Generic;
using System.Linq;

namespace Tactical.DDD
{
    /// <summary>
    /// ValueObject represents value object tactical DDD pattern.
    /// Main properties of value objects is their immutability
    /// and structural equality (two value objects are equal if
    /// their properties are equal) 
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// This is needed as salt for index. If only index was used, there is a chance that i ^ i+some_low_number produces same value
        /// </summary>
        private const int HighPrime = 557927;

        /// <summary>
        /// Override GetAtomicValues in order to implement structural equality for your value object.
        /// </summary>
        /// <returns>Enumerable of properties to participate in equality comparison</returns>
        protected abstract IEnumerable<object> GetAtomicValues();

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select((x, i) => (x != null ? x.GetHashCode() : 0) + (HighPrime * i))
                .Aggregate((x, y) => x ^ y);
        }

        public ValueObject GetCopy()
        {
            return this.MemberwiseClone() as ValueObject;
        }

        public bool Equals(ValueObject obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            return GetHashCode() == obj.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((ValueObject) obj);
        }

        public static bool operator ==(ValueObject lhs, ValueObject rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ValueObject lhs, ValueObject rhs) => !(lhs == rhs);
    }
}