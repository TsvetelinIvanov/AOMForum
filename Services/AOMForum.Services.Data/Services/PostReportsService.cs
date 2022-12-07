using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.PostReports;
using Microsoft.EntityFrameworkCore;
using static AOMForum.Common.GlobalConstants;

namespace AOMForum.Services.Data.Services
{
    public class PostReportsService : IPostReportsService
    {
        private readonly IDeletableEntityRepository<PostReport> postReportsRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;

        public PostReportsService(IDeletableEntityRepository<PostReport> postReportsRepository, IDeletableEntityRepository<Post> postsRepository)
        {
            this.postReportsRepository = postReportsRepository;
            this.postsRepository = postsRepository;
        }

        public async Task<IEnumerable<PostReportListViewModel>> GetPostReportListViewModelsAsync()
        {
            IEnumerable<PostReportListViewModel> viewModels = await this.postReportsRepository.All()
                .Include(pr => pr.Post)
                .Include(pr => pr.Author)
                .AsNoTracking().Select(pr => new PostReportListViewModel()
            {
                Id = pr.Id,
                Content = pr.Content,
                CreatedOn = pr.CreatedOn.ToString(UsedDateTimeFormat),
                PostTitle = pr.Post.Title,
                AuthorUserName = pr.Author.UserName,
                AuthorProfilePictureURL= pr.Author.ProfilePictureURL
            }).ToListAsync();

            return viewModels;
        }

        public async Task<PostReportDetailsViewModel?> GetPostReportDetailsViewModelAsync(int id)
        {
            PostReport? postReport = await this.postReportsRepository.All()
                .Include(pr => pr.Post)
                .Include(pr => pr.Author)
                .AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
            if (postReport == null)
            {
                return null;
            }

            PostReportDetailsViewModel viewModel = new PostReportDetailsViewModel()
            {
                Id = postReport.Id,
                Content = postReport.Content,
                CreatedOn = postReport.CreatedOn.ToString(UsedDateTimeFormat),
                PostId = postReport.PostId,
                PostTitle = postReport.Post.Title,
                AuthorId = postReport.AuthorId,
                AuthorUserName = postReport.Author.UserName,
                AuthorProfilePictureURL = postReport.Author.ProfilePictureURL
            };

            return viewModel;
        }

        public async Task<PostReportInputModel?> GetPostReportInputModelAsync(int postId)
        {
            Post? post = await this.postsRepository.AllAsNoTracking().Where(p => p.Id == postId).FirstOrDefaultAsync();
            if (post == null)
            {
                return null;
            }

            PostReportInputModel inputModel = new PostReportInputModel() { PostId = post.Id };

            return inputModel;
        }

        public async Task<bool> CreateAsync(string? content, int postId, string? authorId)
        {
            Post? post = await this.postsRepository.AllAsNoTracking().Where(p => p.Id == postId).FirstOrDefaultAsync();
            if (post == null)
            {
                return false;
            }

            PostReport postReport = new PostReport
            {
                Content = content,                
                PostId = postId,
                AuthorId = authorId
            };

            await this.postReportsRepository.AddAsync(postReport);
            await this.postReportsRepository.SaveChangesAsync();

            return postReport.CreatedOn != null;
        }

        public async Task<PostReportDeleteModel?> GetPostReportDeleteModelAsync(int id)
        {
            PostReport? postReport = await this.postReportsRepository.All()
                .Include(pr => pr.Post)
                .Include(pr => pr.Author)
                .AsNoTracking().Where(pr => pr.Id == id).FirstOrDefaultAsync();
            if (postReport == null)
            {
                return null;
            }

            PostReportDeleteModel deleteModel = new PostReportDeleteModel()
            {
                Id = postReport.Id,
                Content = postReport.Content,
                CreatedOn = postReport.CreatedOn.ToString(UsedDateTimeFormat),
                PostId = postReport.PostId,
                PostTitle = postReport.Post.Title,
                AuthorUserName = postReport.Author.UserName,
                AuthorProfilePictureURL = postReport.Author.ProfilePictureURL
            };

            return deleteModel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            PostReport? postReport = await this.postReportsRepository.All().Where(pr => pr.Id == id).FirstOrDefaultAsync();
            if (postReport == null)
            {
                return false;
            }

            this.postReportsRepository.Delete(postReport);
            await this.postReportsRepository.SaveChangesAsync();

            return postReport.IsDeleted;
        }
    }
}