using TopList.Entity.Base;

namespace TopList.Services.BaseServices
{
    public interface IEntityService
    {
        string ToSafeSlug(string slug, long entityId, string entityTypeId);

        TopList.Entity.Base.Entity Get(long entityId, string entityTypeId);

        void Add(string name, string slug, long entityId, string entityTypeId);

        void Update(string newName, string newSlug, long entityId, string entityTypeId);

        Task Remove(long entityId, string entityTypeId);
    }
}
