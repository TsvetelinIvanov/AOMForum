using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Repositories;
using AOMForum.Data;
using AOMForum.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using AOMForum.Data.Models;
using AOMForum.Web.Models.Categories;

namespace AOMForum.Services.Data.Tests
{
    public class CategoriesServiceTests
    {
        private const int TestCategoryId = 10;
        private const int TestOtherCategoryId = 11;
        private const int TestCategoryForActionId = 101;
        private const string TestCategoryName = "TestCategoryName";
        private const string TestOtherCategoryName = "TestOtherCategoryName";
        private const string TestCategoryDescription = "TestCategoryDescription";
        private const string TestOtherCategoryDescription = "TestOtherCategoryDescription";
        private const string TestCategoryImageUrl = "TestOtherCategoryImageUrl";
        private const string TestOtherCategoryImageUrl = "TestOtherCategoryImageUrl";
        private const string TestSearch = "Other";
        private const string TestInexistantSearch = "Inexistant";

        private readonly Category testCategory = new Category()
        {
            Id = TestCategoryId,
            Name = TestCategoryName,
            Description = TestCategoryDescription,
            ImageUrl = TestCategoryImageUrl,
            CreatedOn = DateTime.UtcNow,
            IsDeleted = false
        };
        private readonly Category testOtherCategory = new Category()
        {
            Id = TestOtherCategoryId,
            Name = TestOtherCategoryName,
            Description = TestOtherCategoryDescription,
            ImageUrl = TestOtherCategoryImageUrl,
            CreatedOn = DateTime.UtcNow,
            IsDeleted = false
        };
        private readonly Category testCategoryForAction = new Category()
        {
            Id = TestCategoryForActionId,
            Name = TestCategoryName,
            Description = TestCategoryDescription,
            ImageUrl = TestCategoryImageUrl,
            CreatedOn = DateTime.UtcNow,
            IsDeleted = false
        };

        [Fact]
        public async Task GetAllViewModelAsync_ShouldReturnExpectedCategoriesAllViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            CategoriesAllViewModel actualModel = await service.GetAllViewModelAsync();

            Assert.NotNull(actualModel);
            Assert.Equal(2, actualModel.Categories.Count());
            Assert.Contains(actualModel.Categories, c => c.Id == TestCategoryId);
            Assert.Contains(actualModel.Categories, c => c.Id == TestOtherCategoryId);
            Assert.Contains(actualModel.Categories, c => c.Name == TestCategoryName);
            Assert.Contains(actualModel.Categories, c => c.Name == TestOtherCategoryName);
        }

        [Fact]
        public async Task GetAllViewModelAsync_ShouldReturnExpectedCategoriesAllViewModel_WithEmtyCategories()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            
            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            CategoriesAllViewModel actualModel = await service.GetAllViewModelAsync();

            Assert.NotNull(actualModel);
            Assert.Empty(actualModel.Categories);
        }

        [Fact]
        public async Task GetAllViewModelAsync_ShouldReturnExpectedCategoriesAllViewModel_WithSearch()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            CategoriesAllViewModel actualModel = await service.GetAllViewModelAsync(TestSearch);

            Assert.NotNull(actualModel);
            Assert.Equal(TestSearch, actualModel.Search);
            Assert.Single(actualModel.Categories);
            Assert.Contains(actualModel.Categories, c => c.Id == TestOtherCategoryId);
            Assert.Contains(actualModel.Categories, c => c.Name == TestOtherCategoryName);
        }

        [Fact]
        public async Task GetAllViewModelAsync_ShouldReturnExpectedCategoriesAllViewModelWithEmtyCategories_ForInexistantSearch()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            CategoriesAllViewModel actualModel = await service.GetAllViewModelAsync(TestInexistantSearch);

            Assert.NotNull(actualModel);
            Assert.Equal(TestInexistantSearch, actualModel.Search);
            Assert.Empty(actualModel.Categories);
        }

        [Fact]
        public async Task GetDetailsViewModelAsync_ShouldReturnExpectedCategoryDetailsViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            CategoryDetailsViewModel? actualModel = await service.GetDetailsViewModelAsync(TestCategoryId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestCategoryId, actualModel.Id);
            Assert.Equal(TestCategoryName, actualModel.Name);
            Assert.Equal(TestCategoryDescription, actualModel.Description);
            Assert.Equal(0, actualModel.PostsCount);
        }

        [Fact]
        public async Task GetDetailsViewModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            CategoryDetailsViewModel? actualModel = await service.GetDetailsViewModelAsync(TestCategoryForActionId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateCategory()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            int actualCategoryId = await service.CreateAsync(TestCategoryName, TestCategoryDescription, TestCategoryImageUrl);
            Category? category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == TestCategoryName);

            Assert.NotNull(category);
            Assert.Equal(TestCategoryId, category.Id);
            Assert.Equal(TestCategoryDescription, category.Description);
            Assert.Equal(TestCategoryImageUrl, category.ImageUrl);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedCategoryId()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);            

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            int actualCategoryId = await service.CreateAsync(TestCategoryName, TestCategoryDescription, TestCategoryImageUrl);
            Category? category = await dbContext.Categories.FindAsync(actualCategoryId);

            Assert.NotNull(category);
            Assert.Equal(TestCategoryName, category.Name);
            Assert.Equal(TestCategoryDescription, category.Description);
            Assert.Equal(TestCategoryImageUrl, category.ImageUrl);
        }

        [Fact]
        public async Task GetEditModelAsync_ShouldReturnExpectedCategoryEditModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            CategoryEditModel? actualModel = await service.GetEditModelAsync(TestCategoryId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestCategoryId, actualModel.Id);
            Assert.Equal(TestCategoryName, actualModel.Name);
            Assert.Equal(TestCategoryDescription, actualModel.Description);
        }

        [Fact]
        public async Task GetEditModelAsync_ShouldReturnNull_IfInexistantId()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            CategoryEditModel? actualModel = await service.GetEditModelAsync(TestCategoryForActionId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task EditAsync_ShouldEdit()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.Categories.AddAsync(this.testCategoryForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            bool isEdited = await service.EditAsync(TestCategoryForActionId, TestCategoryName, TestOtherCategoryDescription, TestCategoryImageUrl);
            Category? category = await dbContext.Categories.FindAsync(TestCategoryForActionId);

            Assert.NotNull(category);
            Assert.Equal(TestCategoryForActionId, category.Id);
            Assert.Equal(TestCategoryName, category.Name);
            Assert.Equal(TestOtherCategoryDescription, category.Description);
            Assert.Equal(TestCategoryImageUrl, category.ImageUrl);
        }

        [Fact]
        public async Task EditAsync_ShouldReturnTrue_IfEdited()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.Categories.AddAsync(this.testCategoryForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            bool isEdited = await service.EditAsync(TestCategoryForActionId, TestCategoryName, TestOtherCategoryDescription, TestCategoryImageUrl);
            Category? category = await dbContext.Categories.FindAsync(TestCategoryForActionId);

            Assert.True(isEdited);
            Assert.NotNull(category);
            Assert.Equal(TestCategoryForActionId, category.Id);
            Assert.Equal(TestCategoryName, category.Name);
            Assert.Equal(TestOtherCategoryDescription, category.Description);
            Assert.Equal(TestCategoryImageUrl, category.ImageUrl);
        }

        [Fact]
        public async Task EditAsync_ShouldReturnFalse_IfNotEdited()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            bool isEdited = await service.EditAsync(TestCategoryForActionId, TestCategoryName, TestOtherCategoryDescription, TestCategoryImageUrl);
            Category? category = await dbContext.Categories.FindAsync(TestCategoryForActionId);

            Assert.False(isEdited);
            Assert.Null(category);
        }

        [Fact]
        public async Task GetDeleteModelAsync_ShouldReturnExpectedCategoryDeleteModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.Categories.AddAsync(this.testCategoryForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            CategoryDeleteModel? actualModel = await service.GetDeleteModelAsync(TestCategoryForActionId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestCategoryForActionId, actualModel.Id);
            Assert.Equal(TestCategoryName, actualModel.Name);
            Assert.Equal(TestCategoryDescription, actualModel.Description);
            Assert.Equal(0, actualModel.PostsCount);
        }

        [Fact]
        public async Task GetDeleteModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            CategoryDeleteModel? actualModel = await service.GetDeleteModelAsync(TestCategoryForActionId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.Categories.AddAsync(this.testCategoryForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            bool isDeleted = await service.DeleteAsync(TestCategoryForActionId);
            Category? category = await dbContext.Categories.FindAsync(TestCategoryForActionId);

            Assert.NotNull(category);
            Assert.True(category.IsDeleted);
            Assert.Equal(TestCategoryForActionId, category.Id);
            Assert.Equal(TestCategoryName, category.Name);
            Assert.Equal(TestOtherCategoryDescription, category.Description);
            Assert.Equal(TestCategoryImageUrl, category.ImageUrl);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_IfDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.Categories.AddAsync(this.testCategoryForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            bool isDeleted = await service.DeleteAsync(TestCategoryForActionId);
            Category? category = await dbContext.Categories.FindAsync(TestCategoryForActionId);

            Assert.True(isDeleted);
            Assert.NotNull(category);
            Assert.True(category.IsDeleted);
            Assert.Equal(TestCategoryForActionId, category.Id);
            Assert.Equal(TestCategoryName, category.Name);
            Assert.Equal(TestOtherCategoryDescription, category.Description);
            Assert.Equal(TestCategoryImageUrl, category.ImageUrl);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_IfNotDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(this.testCategory);
            await dbContext.Categories.AddAsync(this.testOtherCategory);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Category> repository = new EfDeletableEntityRepository<Category>(dbContext);
            CategoriesService service = new CategoriesService(repository);

            bool isDeleted = await service.DeleteAsync(TestCategoryForActionId);
            Category? category = await dbContext.Categories.FindAsync(TestCategoryForActionId);

            Assert.False(isDeleted);
            Assert.Null(category);
            Assert.Equal(2, dbContext.Categories.Count());
        }
    }
}