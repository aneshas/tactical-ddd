namespace Tactical.DDD
{
    public abstract class DomainIdentity : IDomainIdentity
    {
        public abstract string Identity { get; }

        public bool Equals(DomainIdentity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Identity, other.Identity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DomainIdentity) obj);
        }
        
        public static bool operator ==(DomainIdentity lhs, DomainIdentity rhs)
        {
            if (ReferenceEquals(lhs, null))
                return ReferenceEquals(rhs, null);
            
            return lhs.Equals(rhs);
        }

        public static bool operator !=(DomainIdentity lhs, DomainIdentity rhs) => !(lhs == rhs);
        
        public override int GetHashCode() =>
            (Identity != null ? Identity.GetHashCode() : 0);
    }
}