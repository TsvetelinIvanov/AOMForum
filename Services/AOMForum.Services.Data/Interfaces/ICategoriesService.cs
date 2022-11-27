using AOMForum.Web.Models.Categories;
using System.Threading.Tasks;

namespace AOMForum.Services.Data.Interfaces
{
    public interface ICategoriesService
    {
        Task<bool> IsExistingByIdAsync(int id);

        Task<bool> IsExistingByNameAsync(string? name);

        Task<int> CreateAsync(string? name, string? description, string? imageURL);

        Task<bool> EditAsync(int id, string? name, string? description, string? imageURL);

        Task<bool> DeleteAsync(int id);

        Task<CategoryListViewModel?> GetByIdAsync(int id);

        Task<CategoryListViewModel?> GetByNameAsync(string? name);

        Task<IEnumerable<CategoryListViewModel?>> GetAllAsync(string? search = null);
    }
}