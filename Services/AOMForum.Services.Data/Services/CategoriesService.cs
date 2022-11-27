using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace AOMForum.Services.Data.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<bool> IsExistingByIdAsync(int id) => await this.categoriesRepository.All().AnyAsync(c => c.Id == id);

        public async Task<bool> IsExistingByNameAsync(string? name) => await this.categoriesRepository.All().AnyAsync(c => c.Name == name);

        public async Task<int> CreateAsync(string? name, string? description, string? imageURL)
        {
            Category category = new Category
            {
                Name = name,
                Description = description,
                ImageUrl= imageURL
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return category.Id;
        }

        public async Task<bool> EditAsync(int id, string? name, string? description, string? imageURL)
        {
            Category? category = await this.categoriesRepository.All().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return false;
            }

            category.Name = name;
            category.Description = description;
            category.ImageUrl = imageURL;
            
            await this.categoriesRepository.SaveChangesAsync();

            return category.ModifiedOn != null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Category? category = await this.categoriesRepository.All().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return false;
            }

            this.categoriesRepository.Delete(category);
            await this.categoriesRepository.SaveChangesAsync();

            return category.IsDeleted;
        }

        public async Task<CategoryListViewModel?> GetByIdAsync(int id)
        {
            Category? category = await this.categoriesRepository.All().AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return null;
            }

            CategoryListViewModel? model = new CategoryListViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageURL = category.ImageUrl,
                PostsCount = category.Posts.Count,
                Posts = category.Posts.Select(p => new PostInCategoryViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CommentsCount= p.Comments.Count                    
                })
            };

            return model;
        }

        public async Task<CategoryListViewModel?> GetByNameAsync(string? name)
        {
            Category? category = await this.categoriesRepository.All().AsNoTracking().Where(c => c.Name == name).FirstOrDefaultAsync();
            if (category == null)
            {
                return null;
            }

            CategoryListViewModel? model = new CategoryListViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageURL = category.ImageUrl,
                PostsCount = category.Posts.Count,
                Posts = category.Posts.Select(p => new PostInCategoryViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CommentsCount = p.Comments.Count
                })
            };

            return model;
        }

        public async Task<IEnumerable<CategoryListViewModel?>> GetAllAsync(string? search = null)
        {
            IQueryable<Category> categories = this.categoriesRepository.All().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                categories = categories.Where(c => c.Name != null && c.Name.Contains(search));
            }

            List<CategoryListViewModel> categoryModels = await categories.OrderByDescending(c => c.CreatedOn).Select(c => new CategoryListViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageURL = c.ImageUrl,
                PostsCount = c.Posts.Count,
                Posts = c.Posts.Select(p => new PostInCategoryViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CommentsCount = p.Comments.Count
                })
            }).ToListAsync();

            return categoryModels;
        }
    }
}