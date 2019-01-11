using System.Collections.Generic;
using System.Linq;

namespace Tactical.DDD
{
    public abstract class ValueObject
    {
        public bool Equals(ValueObject other)
        {
            var lhsProps = GetType().GetProperties();
            var rhsProps = other.GetType().GetProperties();

            foreach (var lhsProp in lhsProps)
            {
                var lhs = lhsProp.GetValue(this);
                var rhs = rhsProps.FirstOrDefault(p => p.Name == lhsProp.Name)?.GetValue(other);
                
                if (lhs != rhs && !lhs.Equals(rhs))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValueObject) obj);
        }

        public static bool operator ==(ValueObject lhs, ValueObject rhs)
        {
            if (ReferenceEquals(lhs, null))
                return ReferenceEquals(rhs, null);

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ValueObject lhs, ValueObject rhs) => !(lhs == rhs);

        // ???
        // GetHashcode since its immutable ???
    }
}