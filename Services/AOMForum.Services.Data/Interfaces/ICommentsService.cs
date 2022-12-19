using AOMForum.Web.Models.Comments;
using AOMForum.Web.Models.Posts;

namespace AOMForum.Services.Data.Interfaces
{
    public interface ICommentsService
    {
        Task<CommentDetailsViewModel?> GetCommentDetailsViewModelAsync(int id);

        Task<int> CreateAsync(string? content, /*int? parentId, */int postId, string? authorId);

        Task<string?> GetAuthorIdAsync(int id);

        Task<CommentEditModel?> GetCommentEditModelAsync(int id);

        Task<bool> EditAsync(int id, string? content);

        Task<CommentDeleteModel?> GetCommentDeleteModelAsync(int id);

        Task<bool> DeleteAsync(int id);

        Task<int> GetPostIdAsync(int id);
    }
}