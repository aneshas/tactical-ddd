namespace Tactical.DDD
{
    public interface IEntity<out TIdentity>
        where TIdentity : IEntityId 
    {
        TIdentity Id { get; } 
    }
}