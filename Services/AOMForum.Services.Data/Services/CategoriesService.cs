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

        public async Task<CategoriesAllViewModel> GetAllViewModelAsync(string? search = null)
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
                ImageUrl = c.ImageUrl,
                PostsCount = c.Posts.Count,
                Posts = c.Posts.Select(p => new PostInCategoryViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CommentsCount = p.Comments.Count
                })
            }).ToListAsync();

            CategoriesAllViewModel viewModel = new CategoriesAllViewModel
            {
                Search = search,
                Categories = categoryModels
            };

            return viewModel;
        }

        public async Task<int> CreateAsync(string? name, string? description, string? imageUrl)
        {
            Category category = new Category
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return category.Id;
        }

        public async Task<CategoryEditModel?> GetEditModelAsync(int id)
        {
            Category? category = await this.categoriesRepository.All().AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return null;
            }

            CategoryEditModel? model = new CategoryEditModel()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl
            };

            return model;
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

        public async Task<CategoryDeleteModel?> GetDeleteModelAsync(int id)
        {
            Category? category = await this.categoriesRepository.All().AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return null;
            }

            CategoryDeleteModel? model = new CategoryDeleteModel()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                PostsCount = category.Posts.Count
            };

            return model;
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
    }
}