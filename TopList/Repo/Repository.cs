using TopList.Data;
using TopList.Entity.Base;

namespace TopList.Repo
{
    public class Repository<T> : RepositoryWithTypedId<T, long>, IRepository<T>
      where T : class, IEntityWithTypedId<long>
    {
        public Repository(ApplicationDbContext context) : base(context)
        {
        }
    }
}