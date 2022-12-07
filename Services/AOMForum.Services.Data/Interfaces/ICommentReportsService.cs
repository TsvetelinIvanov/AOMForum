using AOMForum.Web.Models.CommentReports;

namespace AOMForum.Services.Data.Interfaces
{
    public interface ICommentReportsService
    {
        Task<IEnumerable<CommentReportListViewModel>> GetCommentReportListViewModelsAsync();

        Task<CommentReportDetailsViewModel?> GetCommentReportDetailsViewModelAsync(int id);

        Task<CommentReportInputModel?> GetCommentReportInputModelAsync(int commentId);

        Task<bool> CreateAsync(string? content, int postId, string? authorId);

        Task<CommentReportDeleteModel?> GetCommentReportDeleteModelAsync(int id);

        Task<bool> DeleteAsync(int id);
    }
}