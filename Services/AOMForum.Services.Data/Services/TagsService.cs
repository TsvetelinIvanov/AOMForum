using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Posts;
using AOMForum.Web.Models.Tags;
using Microsoft.EntityFrameworkCore;

namespace AOMForum.Services.Data.Services
{
    public class TagsService : ITagsService
    {
        private readonly IDeletableEntityRepository<Tag> tagsRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;

        public TagsService(IDeletableEntityRepository<Tag> tagsRepository, IDeletableEntityRepository<Post> postsRepository)
        {
            this.tagsRepository = tagsRepository;
            this.postsRepository = postsRepository;
        }

        public async Task<int> GetTagsCountAsync(string? search = null)
        {
            IQueryable<Tag> tags = this.tagsRepository.AllAsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                tags = tags.Where(t => t.Name != null && t.Name.Contains(search));
            }

            int count = await tags.CountAsync();

            return count;
        }

        public async Task<IEnumerable<TagListViewModel>> GetAllTagListViewModelsAsync(string? search = null, int skip = 0, int? take = null)
        {
            IQueryable<Tag> tags = this.tagsRepository.All().Include(t => t.Posts).AsNoTracking().OrderByDescending(p => p.CreatedOn);
            if (!string.IsNullOrWhiteSpace(search))
            {
                tags = tags.Where(p => p.Name != null && p.Name.Contains(search));
            }

            if (take.HasValue)
            {
                tags = tags.Skip(skip).Take(take.Value);
            }

            List<TagListViewModel> viewModels = await tags.Select(t => new TagListViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                PostsCount = t.Posts.Count,
            }).ToListAsync();

            return viewModels;
        }

        public TagsAllViewModel GetTagsAllViewModel(int tagsCount, int tagsPerPage, IEnumerable<TagListViewModel> tagModels, int page = 1, string? search = null)
        {
            TagsAllViewModel viewModel = new TagsAllViewModel()
            {
                Search = search,
                PageIndex = page,
                TotalPagesCount = (int)Math.Ceiling(tagsCount / (decimal)tagsPerPage),
                Tags = tagModels
            };

            return viewModel;
        }

        public async Task<TagDetailsViewModel?> GetTagDetailsViewModelAsync(int id)
        {
            Tag? tag = await this.tagsRepository.All().Include(t => t.Posts).AsNoTracking().Where(t => t.Id == id).FirstOrDefaultAsync();
            if (tag == null)
            {
                return null;
            }

            TagDetailsViewModel viewModel = new TagDetailsViewModel()
            {
                Id = tag.Id,
                Name = tag.Name,
                PostsCount = tag.Posts.Count
            };

            List<PostListViewModel> postViewModels = new List<PostListViewModel>();
            foreach (int postId in tag.Posts.Select(pt => pt.PostId))
            {
                Post? post = await this.postsRepository.All()
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .Include(p => p.Reports)
                .Include(p => p.Tags)
                    .ThenInclude(pt => pt.Tag)
                .AsNoTracking().Where(p => p.Id == postId).FirstOrDefaultAsync();
                if (post != null)
                {
                    PostListViewModel postViewModel = new PostListViewModel()
                    {
                        Id = post.Id,
                        Title = post.Title,                        
                        CommentsCount = post.Comments.Count,
                        AuthorId = post.AuthorId,
                        AuthorUserName = post.Author.UserName,
                        AuthorProfilePictureURL = post.Author.ProfilePictureURL,
                        Category = new CategoryInPostViewModel
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

                    postViewModels.Add(postViewModel);
                }
            }

            viewModel.Posts = postViewModels;

            return viewModel;
        }

        public async Task<int> CreateAsync(string? name)
        {
            Tag tag = new Tag
            {
                Name = name
            };

            await this.tagsRepository.AddAsync(tag);
            await this.tagsRepository.SaveChangesAsync();

            return tag.Id;
        }

        public async Task<TagDeleteModel?> GetDeleteModelAsync(int id)
        {
            Tag? tag = await this.tagsRepository.AllAsNoTracking().Where(t => t.Id == id).FirstOrDefaultAsync();
            if (tag == null)
            {
                return null;
            }

            TagDeleteModel deleteModel = new TagDeleteModel()
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return deleteModel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Tag? tag = await this.tagsRepository.All().Where(t => t.Id == id).FirstOrDefaultAsync();
            if (tag == null)
            {
                return false;
            }

            this.tagsRepository.Delete(tag);
            await this.tagsRepository.SaveChangesAsync();

            return tag.IsDeleted;
        }
    }
}