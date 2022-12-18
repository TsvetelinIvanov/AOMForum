using AOMForum.Data.Models.Enums;
using AOMForum.Web.Models.Posts;

namespace AOMForum.Services.Data.Interfaces
{
    public interface IPostsService
    {
        Task<int> GetPostsCountAsync(string? search = null);

        Task<IEnumerable<PostListViewModel>> GetAllPostListViewModelsAsync(string? search = null, int skip = 0, int? take = null);

        PostsAllViewModel GetPostsAllViewModel(int postsCount, int postsPerPage, IEnumerable<PostListViewModel> postModels, int page = 1, string? search = null);

        Task<PostDetailsViewModel?> GetPostDetailsViewModelAsync(int id);

        Task<PostInputModel> GetPostInputModelAsync();

        Task<int> CreateAsync(string? title, string? content, string? imageUrl, string? authorId, int categoryId, IEnumerable<int> tagIds);

        Task<string?> GetAuthorIdAsync(int id);

        Task<PostEditModel?> GetPostEditModelAsync(int id);

        Task<bool> EditAsync(int id, string? title, string? content, string? imageUrl, int categoryId, IEnumerable<int> tagIds);

        Task<PostDeleteModel?> GetPostDeleteModelAsync(int id);

        Task<bool> DeleteAsync(int id);
    }
}