using System.Collections.Generic;

namespace Tactical.DDD
{
    public abstract class Entity<TIdentity>  
        where TIdentity : EntityId 
    {
        /// <summary>
        /// Id defines entity uniqueness and is used for Equality
        /// comparisons and hash code generation.
        /// </summary>
        public TIdentity Id { get; protected set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return EqualityComparer<TIdentity>.Default.Equals(Id, ((Entity<TIdentity>)obj).Id);
        }
   
        public static bool operator ==(Entity<TIdentity> lhs, Entity<TIdentity> rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Entity<TIdentity> lhs, Entity<TIdentity> rhs) => !(lhs == rhs);

        public override int GetHashCode()
        {
            if (Id.Equals(default(TIdentity)))
            {
                return base.GetHashCode();
            }

            return GetType().GetHashCode() ^ Id.GetHashCode();
        }
    }
}