namespace Tactical.DDD
{
    public interface IEntity<out TIdentity>
        where TIdentity : IDomainIdentity 
    {
        TIdentity Id { get; } 
    }
}