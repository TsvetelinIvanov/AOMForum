using AOMForum.Web.Models.Tags;

namespace AOMForum.Services.Data.Interfaces
{
    public interface ITagsService
    {
        Task<int> GetTagsCountAsync(string? search = null);

        Task<IEnumerable<TagListViewModel>> GetAllTagListViewModelsAsync(string? search = null, int skip = 0, int? take = null);

        TagsAllViewModel GetTagsAllViewModel(int tagsCount, int tagsPerPage, IEnumerable<TagListViewModel> tagModels, int page = 1, string? search = null);

        Task<TagDetailsViewModel?> GetTagDetailsViewModelAsync(int id);

        Task<int> CreateAsync(string? name);

        Task<TagDeleteModel?> GetDeleteModelAsync(int id);

        Task<bool> DeleteAsync(int id);
    }
}