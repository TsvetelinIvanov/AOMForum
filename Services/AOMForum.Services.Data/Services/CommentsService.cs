using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Comments;
using Microsoft.EntityFrameworkCore;
using static AOMForum.Common.GlobalConstants;

namespace AOMForum.Services.Data.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task<CommentDetailsViewModel?> GetCommentDetailsViewModelAsync(int id)
        {
            Comment? comment = await this.commentsRepository.All()
                .Include(c => c.Author)
                .Include(c => c.Post)
                .Include(c => c.Votes)
                .AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (comment == null)
            {
                return null;
            }

            CommentDetailsViewModel viewModel = new CommentDetailsViewModel()
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn.ToString(UsedDateTimeFormat),
                AuthorId = comment.AuthorId,
                AuthorUserName = comment.Author.UserName,
                AuthorProfilePictureURL = comment.Author.ProfilePictureURL,
                PostId = comment.PostId,
                PostAuthorId = comment.Post.AuthorId,
                VotesCount = comment.Votes.Count()
            };

            List<CommentDetailsViewModel> postComments = await this.commentsRepository.All()
                .Include(c => c.Author)
                .Include(c => c.Post)
                .Include(c => c.Votes)
                .AsNoTracking().Where(c => c.ParentId == viewModel.Id).Select(c => new CommentDetailsViewModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedOn = c.CreatedOn.ToString(UsedDateTimeFormat),
                    AuthorId = c.AuthorId,
                    AuthorUserName = c.Author.UserName,
                    AuthorProfilePictureURL = c.Author.ProfilePictureURL,
                    PostId = c.PostId,
                    PostAuthorId = c.Post.AuthorId,
                    VotesCount = c.Votes.Count()
                }).ToListAsync();

            viewModel.Comments = postComments;

            return viewModel;
        }
        
        public async Task<int> CreateAsync(string? content, int? parentId, int postId, string? authorId)
        {
            Comment comment = new Comment
            {
                Content = content,
                ParentId = parentId,
                PostId = postId,
                AuthorId = authorId
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();

            return comment.Id;
        }

        public async Task<string?> GetAuthorIdAsync(int id) => await this.commentsRepository.AllAsNoTracking().Where(c => c.Id == id).Select(c => c.AuthorId).FirstOrDefaultAsync();

        public async Task<CommentEditModel?> GetCommentEditModelAsync(int id)
        {
            Comment? comment = await this.commentsRepository.AllAsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (comment == null)
            {
                return null;
            }

            CommentEditModel editModel = new CommentEditModel()
            {
                Id = comment.Id,                
                Content = comment.Content
            };

            return editModel;
        }

        public async Task<bool> EditAsync(int id, string? content)
        {
            Comment? comment = await this.commentsRepository.AllAsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (comment == null)
            {
                return false;
            }

            comment.Content = content;

            await this.commentsRepository.SaveChangesAsync();

            return comment.ModifiedOn != null;
        }

        public async Task<CommentDeleteModel?> GetCommentDeleteModelAsync(int id)
        {
            Comment? comment = await this.commentsRepository.All()
                .Include(c => c.Author)
                .Include(c => c.Post)
                .Include(c => c.Votes)
                .AsNoTracking()
                .Where(c => c.Id == id).FirstOrDefaultAsync();
            if (comment == null)
            {
                return null;
            }

            CommentDeleteModel deleteModel = new CommentDeleteModel()
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn.ToString(UsedDateTimeFormat),
                AuthorUserName = comment.Author.UserName,
                AuthorProfilePictureURL = comment.Author.ProfilePictureURL,
                PostId = comment.PostId,
                PostTitle = comment.Post.Title,
                VotesCount = comment.Votes.Count(),
                ChildrenCount = this.commentsRepository.AllAsNoTracking().Where(c => c.ParentId == comment.Id).Count()
            };

            return deleteModel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Comment? comment = await this.commentsRepository.All().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (comment == null)
            {
                return false;
            }

            this.commentsRepository.Delete(comment);
            await this.commentsRepository.SaveChangesAsync();

            await this.DeleteNestedAsync(id);

            return comment.IsDeleted;
        }

        public async Task<int> GetPostIdAsync(int id) => await this.commentsRepository.AllAsNoTracking().Where(c => c.Id == id).Select(c => c.PostId).FirstOrDefaultAsync();

        private async Task DeleteNestedAsync(int id)
        {
            //Comment? nestedComment = await this.commentsRepository.All().Where(c => c.Id == id).FirstOrDefaultAsync();
            //if (nestedComment == null)
            //{
            //    return;
            //}

            //this.commentsRepository.Delete(nestedComment);
            //await this.commentsRepository.SaveChangesAsync();

            //await this.DeleteNestedAsync(nestedComment.Id);
            IEnumerable<Comment> nestedComments = await this.commentsRepository.All().Where(c => c.ParentId == id).ToListAsync();
            if (!nestedComments.Any())
            {
                return;
            }

            foreach (Comment nestedComment in nestedComments)
            {
                this.commentsRepository.Delete(nestedComment);
                await this.commentsRepository.SaveChangesAsync();

                await this.DeleteNestedAsync(nestedComment.Id);
            }
        }
    }
}