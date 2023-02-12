using TopList.Entity.EntityModels;
using TopList.ViewModels;

namespace TopList.Services
{
    public interface ICategoryService
    {
        Task<IList<CategoryListItem>> GetAll();

        Task Create(Category category);

        Task Update(Category category);

        Task Delete(Category category);
    }
}

