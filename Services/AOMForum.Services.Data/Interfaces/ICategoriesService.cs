using AOMForum.Web.Models.Categories;
using System.Threading.Tasks;

namespace AOMForum.Services.Data.Interfaces
{
    public interface ICategoriesService
    {
        Task<CategoriesAllViewModel> GetAllViewModelAsync(string? search = null);

        Task<CategoryDetailsViewModel?> GetDetailsViewModelAsync(int id);

        Task<int> CreateAsync(string? name, string? description, string? imageUrl);

        Task<CategoryEditModel?> GetEditModelAsync(int id);

        Task<bool> EditAsync(int id, string? name, string? description, string? imageUrl);

        Task<CategoryDeleteModel?> GetDeleteModelAsync(int id);

        Task<bool> DeleteAsync(int id);
    }
}