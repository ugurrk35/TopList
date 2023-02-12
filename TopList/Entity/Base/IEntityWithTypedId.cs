namespace TopList.Entity.Base
{
    public interface IEntityWithTypedId<TId>
    {
        TId Id { get; }
    }
}
