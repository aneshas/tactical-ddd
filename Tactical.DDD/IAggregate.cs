namespace Tactical.DDD
{
    public interface IAggregate<out TIdentity> : IEntity<TIdentity>
        where TIdentity : IDomainIdentity 
    {
        
    }
}