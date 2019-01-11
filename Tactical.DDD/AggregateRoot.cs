namespace Tactical.DDD
{
    public class AggregateRoot<TIdentity> : Entity<TIdentity>
        where TIdentity : IDomainIdentity 
    {
    }
}