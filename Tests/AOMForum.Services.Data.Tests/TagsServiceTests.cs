using AOMForum.Data.Common.Repositories;
using AOMForum.Data;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using AOMForum.Data.Repositories;
using AOMForum.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using AOMForum.Web.Models.Posts;
using AOMForum.Web.Models.Tags;
using AOMForum.Web.Models.Categories;

namespace AOMForum.Services.Data.Tests
{
    public class TagsServiceTests
    {
        private const int TestTagId = 10;
        private const int TestOtherTagId = 11;
        private const int TestInexistantTagId = 101;
        private const string TestPostAuthorId = "TestPostAuthorId";
        private const string TestTagName = "Test Tag Name";
        private const string TestOtherTagName = "Test Other Tag Name";
        private const string TestSearch = "Other";
        private const string TestInexistantSearch = "Inexistant";

        private readonly Tag testTag = new Tag()
        {
            Id = TestTagId,
            Name = TestTagName
        };

        private readonly Tag testOtherTag = new Tag()
        {
            Id = TestOtherTagId,
            Name = TestOtherTagName
        };

        [Fact]
        public async Task GetPostsCountAsync_ShouldReturnPostsCount()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            int tagsCount = await service.GetTagsCountAsync();

            Assert.Equal(2, tagsCount);
        }

        [Fact]
        public async Task GetPostsCountAsync_ShouldReturnPostsCount_WithSearch()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            int tagsCount = await service.GetTagsCountAsync(TestSearch);

            Assert.Equal(1, tagsCount);
        }

        [Fact]
        public async Task GetPostsCountAsync_ShouldReturnZeroCount_WithInexistantSearch()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            int tagsCount = await service.GetTagsCountAsync(TestInexistantSearch);

            Assert.Equal(0, tagsCount);
        }

        [Fact]
        public async Task GetAllTagListViewModelsAsync_ShouldReturnExpectedTagListViewModels()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            IEnumerable<TagListViewModel> actualModels = await service.GetAllTagListViewModelsAsync();

            Assert.NotNull(actualModels);
            Assert.Equal(2, actualModels.Count());
            Assert.Contains(actualModels, t => t.Id == TestTagId);
            Assert.Contains(actualModels, t => t.Id == TestOtherTagId);
        }

        [Fact]
        public async Task GetAllTagListViewModelsAsync_ShouldReturnExpectedTagListViewModels_WithSearch()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            IEnumerable<TagListViewModel> actualModels = await service.GetAllTagListViewModelsAsync(TestSearch);

            Assert.NotNull(actualModels);
            Assert.Single(actualModels);
            Assert.Contains(actualModels, t => t.Id == TestOtherTagId);
        }

        [Fact]
        public async Task GetAllTagListViewModelsAsync_ShouldReturnExpectedTagListViewModelsWithEmtyTags_ForInexistantSearch()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            IEnumerable<TagListViewModel> actualModels = await service.GetAllTagListViewModelsAsync(TestInexistantSearch);

            Assert.NotNull(actualModels);
            Assert.Empty(actualModels);
        }

        [Fact]
        public async Task GetTagsAllViewModel_ShouldReturnExpectedTagsAllViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            IEnumerable<TagListViewModel> actualModels = await service.GetAllTagListViewModelsAsync();
            TagsAllViewModel actualModel = service.GetTagsAllViewModel(2, 2, actualModels);

            Assert.NotNull(actualModel);
            Assert.Equal(2, actualModel.Tags.Count());
        }

        [Fact]
        public async Task GetTagDetailsViewModelAsync_ShouldReturnExpectedTagDetailsViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            TagDetailsViewModel? actualModel = await service.GetTagDetailsViewModelAsync(TestTagId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestTagId, actualModel.Id);
            Assert.Equal(TestTagName, actualModel.Name);
        }

        [Fact]
        public async Task GetTagDetailsViewModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            TagDetailsViewModel? actualModel = await service.GetTagDetailsViewModelAsync(TestInexistantTagId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateTag()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            int actualTagId = await service.CreateAsync(TestTagName);
            Tag? tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Name == TestTagName);

            Assert.NotNull(tag);
            Assert.Equal(TestTagId, tag.Id);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedTagId()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            int actualTagId = await service.CreateAsync(TestTagName);
            Tag? tag = await dbContext.Tags.FindAsync(actualTagId);

            Assert.NotNull(tag);
            Assert.Equal(TestTagName, tag.Name);
        }

        [Fact]
        public async Task GetDeleteModelAsync_ShouldReturnExpectedTagDeleteModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            TagDeleteModel? actualModel = await service.GetDeleteModelAsync(TestTagId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestTagId, actualModel.Id);
            Assert.Equal(TestTagName, actualModel.Name);
        }

        [Fact]
        public async Task GetDeleteModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            TagDeleteModel? actualModel = await service.GetDeleteModelAsync(TestInexistantTagId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            bool isDeleted = await service.DeleteAsync(TestTagId);
            Tag? tag = await dbContext.Tags.FindAsync(TestTagId);

            Assert.NotNull(tag);
            Assert.True(tag.IsDeleted);
            Assert.Equal(TestTagName, tag.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_IfDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            bool isDeleted = await service.DeleteAsync(TestTagId);
            Tag? tag = await dbContext.Tags.FindAsync(TestTagId);

            Assert.True(isDeleted);
            Assert.NotNull(tag);
            Assert.True(tag.IsDeleted);
            Assert.Equal(TestTagName, tag.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_IfNotDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Tags.AddAsync(this.testTag);
            await dbContext.Tags.AddAsync(this.testOtherTag);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            TagsService service = new TagsService(tagsRepository, postsRepository);

            bool isDeleted = await service.DeleteAsync(TestInexistantTagId);
            Tag? tag = await dbContext.Tags.FindAsync(TestInexistantTagId);

            Assert.False(isDeleted);
            Assert.Null(tag);
            Assert.Equal(2, dbContext.Tags.Count());
        }
    }
}