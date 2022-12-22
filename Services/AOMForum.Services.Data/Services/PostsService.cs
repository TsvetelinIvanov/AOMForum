using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Posts;
using Microsoft.EntityFrameworkCore;
using static AOMForum.Common.GlobalConstants;

namespace AOMForum.Services.Data.Services
{
    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Tag> tagsRepository;
        private readonly IDeletableEntityRepository<PostTag> postsTagsRepository;

        public PostsService(IDeletableEntityRepository<Post> postsRepository, IDeletableEntityRepository<Category> categoriesRepository, IDeletableEntityRepository<Tag> tagsRepository, IDeletableEntityRepository<PostTag> postsTagsRepository)
        {
            this.postsRepository = postsRepository;
            this.categoriesRepository = categoriesRepository;
            this.tagsRepository = tagsRepository;
            this.postsTagsRepository = postsTagsRepository;
        }

        public async Task<int> GetPostsCountAsync(string? search = null)
        {
            IQueryable<Post> posts = this.postsRepository.AllAsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                posts = posts.Where(t => t.Title != null && t.Title.Contains(search));
            }

            int count = await posts.CountAsync();

            return count;
        }

        public async Task<IEnumerable<PostListViewModel>> GetAllPostListViewModelsAsync(string? search = null, int skip = 0, int? take = null)
        {
            IQueryable<Post> posts = this.postsRepository.All()
                .Include(p => p.Author)
                .Include(p => p.Category)                
                .Include(p => p.Comments)
                .Include(p => p.Reports)
                .Include(p => p.Tags)
                    .ThenInclude(pt => pt.Tag)
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedOn);
            if (!string.IsNullOrWhiteSpace(search))
            {
                posts = posts.Where(p => p.Title != null && p.Title.Contains(search));
            }

            if (take.HasValue)
            {
                posts = posts.Skip(skip).Take(take.Value);
            }

            List<PostListViewModel> viewModels = await posts.Select(p => new PostListViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                ImageUrl = p.ImageUrl,
                CommentsCount = p.Comments.Count,
                AuthorId = p.AuthorId,
                AuthorUserName = p.Author.UserName,
                AuthorProfilePictureURL = p.Author.ProfilePictureURL,
                Category = new CategoryInPostViewModel
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name
                },
                Tags = p.Tags.Select(pt => new TagInPostViewModel
                {
                    Id = pt.TagId,
                    Name = pt.Tag.Name
                })
            }).ToListAsync();

            return viewModels;
        }

        public PostsAllViewModel GetPostsAllViewModel(int postsCount, int postsPerPage, IEnumerable<PostListViewModel> postModels, int page = 1, string? search = null)
        {
            PostsAllViewModel viewModel = new PostsAllViewModel
            {
                Search = search,
                PageIndex = page,
                TotalPagesCount = (int)Math.Ceiling(postsCount / (decimal)postsPerPage),
                Posts = postModels
            };

            return viewModel;
        }

        public async Task<PostDetailsViewModel?> GetPostDetailsViewModelAsync(int id)
        {
            Post? post = await this.postsRepository.All()
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .Include(p => p.Reports)
                //.Include(p => p.Votes)
                .Include(p => p.Tags)
                    .ThenInclude(pt => pt.Tag)
                .AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
            if (post == null)
            {
                return null;
            }

            Post? postWithComents = await this.postsRepository.All().Include(p => p.Comments).ThenInclude(c => c.Author).AsNoTracking()
                .Where(p => p.Id == id).FirstOrDefaultAsync();
            if (postWithComents == null)
            {
                return null;
            }            

            PostDetailsViewModel viewModel = new PostDetailsViewModel()
            {
                Id = post.Id,
                Title = post.Title,
                CreatedOn = post.CreatedOn.ToString(UsedDateTimeFormat),
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                CommentsCount = post.Comments.Count,
                //VotesCount = post.Votes.Sum(v => (int)v.Type),
                AuthorId = post.AuthorId,
                AuthorUserName = post.Author.UserName,
                AuthorProfilePictureURL = post.Author.ProfilePictureURL,
                Category = new CategoryInPostViewModel()
                {
                    Id = post.Category.Id,
                    Name = post.Category.Name
                },
                Tags = post.Tags.Select(pt => new TagInPostViewModel
                {
                    Id = pt.TagId,
                    Name = pt.Tag.Name
                }),
                Comments = postWithComents.Comments.Select(c => new CommentInPostViewModel()
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedOn = c.CreatedOn.ToString(UsedDateTimeFormat),
                    //ParentId = c.ParentId,
                    AuthorId = c.AuthorId,
                    AuthorUserName = c.Author.UserName,
                    AuthorProfilePictureURL = c.Author.ProfilePictureURL
                })
            };

            return viewModel;
        }

        public async Task<PostInputModel> GetPostInputModelAsync()
        {
            IEnumerable<CategoryInPostViewModel> categoryModels = await this.categoriesRepository.AllAsNoTracking().Select(c => new CategoryInPostViewModel()
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();

            IEnumerable<TagInPostViewModel> tagModels = await this.tagsRepository.AllAsNoTracking().Select(t => new TagInPostViewModel()
            {
                Id = t.Id,
                Name = t.Name
            }).ToListAsync();

            PostInputModel inputModel = new PostInputModel()
            {
                Categories = categoryModels,
                Tags = tagModels
            };

            return inputModel;
        }

        public async Task<int> CreateAsync(string? title, string? content, string? imageUrl, string? authorId, int categoryId, IEnumerable<int> tagIds)
        {
            Post post = new Post
            {
                Title = title,
                Type = PostType.Text,
                Content = content,
                ImageUrl = imageUrl,
                AuthorId = authorId,
                CategoryId = categoryId
            };

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();

            //foreach (int tagId in tagIds)
            //{
            //    post.Tags.Add(new PostTag
            //    {
            //        PostId = post.Id,
            //        TagId = tagId
            //    });
            //}

            //await this.postsRepository.SaveChangesAsync();
            foreach (int tagId in tagIds)
            {
                await this.postsTagsRepository.AddAsync(new PostTag
                {
                    PostId = post.Id,
                    TagId = tagId
                });
            }

            await this.postsTagsRepository.SaveChangesAsync();

            return post.Id;
        }

        public async Task<string?> GetAuthorIdAsync(int id) => await this.postsRepository.AllAsNoTracking().Where(p => p.Id == id).Select(p => p.AuthorId).FirstOrDefaultAsync();

        public async Task<PostEditModel?> GetPostEditModelAsync(int id)
        {
            Post? post = await this.postsRepository.All().Include(p => p.Tags).AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
            if (post == null)
            {
                return null;
            }

            IEnumerable<CategoryInPostViewModel> categoryModels = await this.categoriesRepository.AllAsNoTracking().Select(c => new CategoryInPostViewModel()
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();

            IEnumerable<TagInPostViewModel> tagModels = await this.tagsRepository.AllAsNoTracking().Select(t => new TagInPostViewModel()
            {
                Id = t.Id,
                Name = t.Name
            }).ToListAsync();

            PostEditModel editModel = new PostEditModel()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl= post.ImageUrl,
                CategoryId = post.CategoryId,
                TagIds = post.Tags.Select(t => t.Id),
                Categories = categoryModels,
                Tags = tagModels
            };

            return editModel;
        }

        public async Task<bool> EditAsync(int id, string? title, string? content, string? imageUrl, int categoryId, IEnumerable<int> tagIds)
        {
            Post? post = await this.postsRepository.All().Include(p => p.Tags).Where(p => p.Id == id).FirstOrDefaultAsync();
            if (post == null)
            {
                return false;
            }
            
            List<PostTag> postsTags = await this.postsTagsRepository.All().Where(pt => pt.PostId == id).ToListAsync();
            foreach (PostTag postTag in postsTags)
            {
                this.postsTagsRepository.Delete(postTag);
            }

            await this.postsTagsRepository.SaveChangesAsync();

            post.Title = title;
            post.Content = content;
            post.ImageUrl = imageUrl;
            post.CategoryId = categoryId;
            //foreach (int tagId in tagIds)
            //{
            //    post.Tags.Add(new PostTag
            //    {
            //        PostId = post.Id,
            //        TagId = tagId
            //    });
            //}

            await this.postsRepository.SaveChangesAsync();

            foreach (int tagId in tagIds)
            {
                await this.postsTagsRepository.AddAsync(new PostTag
                {
                    PostId = post.Id,
                    TagId = tagId
                });
            }

            await this.postsTagsRepository.SaveChangesAsync();

            return post.ModifiedOn != null;
        }

        public async Task<PostDeleteModel?> GetPostDeleteModelAsync(int id)
        {
            Post? post = await this.postsRepository.All()
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .Include(p => p.Reports)
                .Include(p => p.Tags)
                    .ThenInclude(pt => pt.Tag)
                .AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (post == null)
            {
                return null;
            }

            PostDeleteModel deleteModel = new PostDeleteModel()
            {
                Id = post.Id,
                Title = post.Title,
                CreatedOn = post.CreatedOn.ToString(UsedDateTimeFormat),
                Content = post.Content,
                CommentsCount = post.Comments.Count,
                AuthorUserName = post.Author.UserName,
                AuthorProfilePictureURL = post.Author.ProfilePictureURL,
                Category = new CategoryInPostViewModel()
                {
                    Id = post.Category.Id,
                    Name = post.Category.Name
                },
                Tags = post.Tags.Select(pt => new TagInPostViewModel
                {
                    Id = pt.TagId,
                    Name = pt.Tag.Name
                })
            };

            return deleteModel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Post? post = await this.postsRepository.All().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (post == null)
            {
                return false;
            }

            this.postsRepository.Delete(post);
            await this.postsRepository.SaveChangesAsync();

            return post.IsDeleted;
        }
    }
}