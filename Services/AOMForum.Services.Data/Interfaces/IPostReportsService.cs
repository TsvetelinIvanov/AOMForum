using AOMForum.Web.Models.PostReports;

namespace AOMForum.Services.Data.Interfaces
{
    public interface IPostReportsService
    {
        Task<IEnumerable<PostReportListViewModel>> GetPostReportListViewModelsAsync();

        Task<PostReportDetailsViewModel?> GetPostReportDetailsViewModelAsync(int id);

        Task<PostReportInputModel?> GetPostReportInputModelAsync(int postId);

        Task<bool> CreateAsync(string? content, int postId, string? authorId);

        Task<PostReportDeleteModel?> GetPostReportDeleteModelAsync(int id);

        Task<bool> DeleteAsync(int id);
    }
}