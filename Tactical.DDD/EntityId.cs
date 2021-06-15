namespace Tactical.DDD
{
    public abstract record EntityId
    {
        public static implicit operator string(EntityId id) => id.ToString();
    }
}