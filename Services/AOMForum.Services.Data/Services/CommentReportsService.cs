using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.CommentReports;
using AOMForum.Web.Models.PostReports;
using Microsoft.EntityFrameworkCore;
using static AOMForum.Common.GlobalConstants;

namespace AOMForum.Services.Data.Services
{
    public class CommentReportsService : ICommentReportsService
    {
        private readonly IDeletableEntityRepository<CommentReport> commentReportsRepository;
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentReportsService(IDeletableEntityRepository<CommentReport> commentReportsRepository, IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentReportsRepository = commentReportsRepository;
            this.commentsRepository = commentsRepository;
        }

        public async Task<IEnumerable<CommentReportListViewModel>> GetCommentReportListViewModelsAsync()
        {
            IEnumerable<CommentReportListViewModel> viewModels = await this.commentReportsRepository.All()
                .Include(cr => cr.Comment)
                .Include(cr => cr.Author)
                .AsNoTracking().Select(cr => new CommentReportListViewModel()
                {
                    Id = cr.Id,
                    Content = cr.Content,
                    CreatedOn = cr.CreatedOn.ToString(UsedDateTimeFormat),
                    CommentContent = cr.Content,
                    AuthorUserName = cr.Author.UserName,
                    AuthorProfilePictureURL = cr.Author.ProfilePictureURL
                }).ToListAsync();

            return viewModels;
        }

        public async Task<CommentReportDetailsViewModel?> GetCommentReportDetailsViewModelAsync(int id)
        {
            CommentReport? commentReport = await this.commentReportsRepository.All()
                .Include(cr => cr.Comment)
                .Include(cr => cr.Author)
                .AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (commentReport == null)
            {
                return null;
            }

            CommentReportDetailsViewModel viewModel = new CommentReportDetailsViewModel()
            {
                Id = commentReport.Id,
                Content = commentReport.Content,
                CreatedOn = commentReport.CreatedOn.ToString(UsedDateTimeFormat),
                CommentId = commentReport.CommentId,
                CommentContent = commentReport.Comment.Content,
                AuthorId = commentReport.AuthorId,
                AuthorUserName = commentReport.Author.UserName,
                AuthorProfilePictureURL = commentReport.Author.ProfilePictureURL
            };

            return viewModel;
        }

        public async Task<CommentReportInputModel?> GetCommentReportInputModelAsync(int commentId)
        {
            Comment? comment = await this.commentsRepository.AllAsNoTracking().Where(c => c.Id == commentId).FirstOrDefaultAsync();
            if (comment == null)
            {
                return null;
            }

            CommentReportInputModel inputModel = new CommentReportInputModel() { CommentId = comment.Id };

            return inputModel;
        }

        public async Task<bool> CreateAsync(string? content, int commentId, string? authorId)
        {
            Comment? comment = await this.commentsRepository.AllAsNoTracking().Where(c => c.Id == commentId).FirstOrDefaultAsync();
            if (comment == null)
            {
                return false;
            }

            CommentReport commentReport = new CommentReport
            {
                Content = content,
                CommentId = commentId,
                AuthorId = authorId
            };

            await this.commentReportsRepository.AddAsync(commentReport);
            await this.commentReportsRepository.SaveChangesAsync();

            return commentReport.CreatedOn != null;
        }

        public async Task<CommentReportDeleteModel?> GetCommentReportDeleteModelAsync(int id)
        {
            CommentReport? commentReport = await this.commentReportsRepository.All()
                .Include(cr => cr.Comment)
                .Include(cr => cr.Author)
                .AsNoTracking().Where(cr => cr.Id == id).FirstOrDefaultAsync();
            if (commentReport == null)
            {
                return null;
            }

            CommentReportDeleteModel deleteModel = new CommentReportDeleteModel()
            {
                Id = commentReport.Id,
                Content = commentReport.Content,
                CreatedOn = commentReport.CreatedOn.ToString(UsedDateTimeFormat),
                CommentId = commentReport.CommentId,
                CommentContent = commentReport.Comment.Content,
                AuthorUserName = commentReport.Author.UserName,
                AuthorProfilePictureURL = commentReport.Author.ProfilePictureURL
            };

            return deleteModel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            CommentReport? commentReport = await this.commentReportsRepository.All().Where(cr => cr.Id == id).FirstOrDefaultAsync();
            if (commentReport == null)
            {
                return false;
            }

            this.commentReportsRepository.Delete(commentReport);
            await this.commentReportsRepository.SaveChangesAsync();

            return commentReport.IsDeleted;
        }
    }
}