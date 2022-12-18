using AOMForum.Data.Common.Repositories;
using AOMForum.Data;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using AOMForum.Data.Repositories;
using AOMForum.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using AOMForum.Web.Models.Posts;

namespace AOMForum.Services.Data.Tests
{
    public class PostsServiceTests
    {
        private const int TestPostId = 10;
        private const int TestOtherPostId = 11;
        private const int TestPostForActionId = 101;        
        private const int TestCategoryPostId = 100;
        private const string TestPostAuthorId = "TestPostAuthorId";
        private const string TestOtherPostAuthorId = "TestOtherPostAuthorId";        
        private const string TestPostTitle = "Test Post Title";
        private const string TestOtherPostTitle = "Test Other Post Title";
        private const string TestPostContent = "Test Post Content";
        private const string TestOtherPostContent = "Test Other Post Content";
        private const string TestSearch = "Other";
        private const string TestInexistantSearch = "Inexistant";

        private readonly Post testPost = new Post()
        {
            Id = TestPostId,
            Title = TestPostTitle,
            Type = PostType.Text,
            ImageUrl = "TestPostImageUrl",
            Content = TestPostContent,
            AuthorId = TestPostAuthorId,
            Author = new ApplicationUser()
            {
                Id = TestPostAuthorId,
                UserName = "TestAuthor",
                Email = "testauthor@mail.com",
                FirstName = "Test",
                SecondName = "Comment",
                LastName = "Author",
                BirthDate = new DateTime(1999, 1, 1),
                Age = 23,
                Gender = GenderType.Male,
                Biography = "Test Author Biography",
                ProfilePictureURL = "ProfilePictureURL",
                EmailConfirmed = true
            },
            CategoryId = TestCategoryPostId,
            Category = new Category()
            {
                Id = TestCategoryPostId,
                Name = "Test Category Name",
                Description = "Test Category Description",
                ImageUrl = "TestCategoryImageUrl"
            }
        };

        private readonly Post testOtherPost = new Post()
        {
            Id = TestOtherPostId,
            Title = TestOtherPostTitle,
            Type = PostType.Text,
            ImageUrl = "TestPostImageUrl",
            Content = TestOtherPostContent,
            AuthorId = TestOtherPostAuthorId,
            Author = new ApplicationUser()
            {
                Id = TestOtherPostAuthorId,
                UserName = "TestOtherAuthor",
                Email = "testauthor@mail.com",
                FirstName = "Test",
                SecondName = "OtherPost",
                LastName = "Author",
                BirthDate = new DateTime(1999, 1, 1),
                Age = 23,
                Gender = GenderType.Male,
                Biography = "Test Author Biography",
                ProfilePictureURL = "ProfilePictureURL",
                EmailConfirmed = true
            },
            CategoryId = TestCategoryPostId,
            Category = new Category()
            {
                Id = TestCategoryPostId,
                Name = "Test Category Name",
                Description = "Test Category Description",
                ImageUrl = "TestCategoryImageUrl"
            }
        };

        private readonly Post testPostForAction = new Post()
        {
            Id = TestPostForActionId,
            Title = TestPostTitle,
            Type = PostType.Text,
            ImageUrl = "TestPostImageUrl",
            Content = TestPostContent,
            AuthorId = TestPostAuthorId,
            Author = new ApplicationUser()
            {
                Id = TestPostAuthorId,
                UserName = "TestAuthor",
                Email = "testauthor@mail.com",
                FirstName = "Test",
                SecondName = "Comment",
                LastName = "Author",
                BirthDate = new DateTime(1999, 1, 1),
                Age = 23,
                Gender = GenderType.Male,
                Biography = "Test Author Biography",
                ProfilePictureURL = "ProfilePictureURL",
                EmailConfirmed = true
            },
            CategoryId = TestCategoryPostId,
            Category = new Category()
            {
                Id = TestCategoryPostId,
                Name = "Test Category Name",
                Description = "Test Category Description",
                ImageUrl = "TestCategoryImageUrl"
            }
        };

        [Fact]
        public async Task GetPostsCountAsync_ShouldReturnPostsCount()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            int postsCount = await service.GetPostsCountAsync();
            
            Assert.Equal(3, postsCount);
        }

        [Fact]
        public async Task GetPostsCountAsync_ShouldReturnPostsCount_WithSearch()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            int postsCount = await service.GetPostsCountAsync(TestSearch);

            Assert.Equal(1, postsCount);
        }

        [Fact]
        public async Task GetPostsCountAsync_ShouldReturnZeroCount_WithInexistantSearch()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            int postsCount = await service.GetPostsCountAsync(TestInexistantSearch);

            Assert.Equal(0, postsCount);
        }

        [Fact]
        public async Task GetAllPostListViewModelsAsync_ShouldReturnExpectedPostListViewModels()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            IEnumerable<PostListViewModel> actualModels = await service.GetAllPostListViewModelsAsync();

            Assert.NotNull(actualModels);
            Assert.Equal(3, actualModels.Count());
            Assert.Contains(actualModels, p => p.Id == TestPostId);
            Assert.Contains(actualModels, p => p.Id == TestOtherPostId);
            Assert.Contains(actualModels, p => p.Id == TestPostForActionId);
        }

        [Fact]
        public async Task GetAllPostListViewModelsAsync_ShouldReturnExpectedPostListViewModels_WithSearch()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            IEnumerable<PostListViewModel> actualModels = await service.GetAllPostListViewModelsAsync(TestSearch);

            Assert.NotNull(actualModels);
            Assert.Single(actualModels);
            Assert.Contains(actualModels, p => p.Id == TestOtherPostId);
        }

        [Fact]
        public async Task GetAllPostListViewModelsAsync_ShouldReturnExpectedPostListViewModelsWithEmtyPosts_ForInexistantSearch()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            IEnumerable<PostListViewModel> actualModels = await service.GetAllPostListViewModelsAsync(TestInexistantSearch);

            Assert.NotNull(actualModels);
            Assert.Empty(actualModels);
        }

        [Fact]
        public async Task GetPostsAllViewModel_ShouldReturnExpectedPostsAllViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            IEnumerable<PostListViewModel> actualModels = await service.GetAllPostListViewModelsAsync();
            PostsAllViewModel actualModel = service.GetPostsAllViewModel(3, 3, actualModels);

            Assert.NotNull(actualModel);
            Assert.Equal(3, actualModel.Posts.Count());
        }

        [Fact]
        public async Task GetPostDetailsViewModelAsync_ShouldReturnExpectedPostDetailsViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            PostDetailsViewModel? actualModel = await service.GetPostDetailsViewModelAsync(TestPostId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestPostId, actualModel.Id);
            Assert.Equal(TestPostTitle, actualModel.Title);
            Assert.Equal(TestPostContent, actualModel.Content);
            Assert.Equal(TestPostAuthorId, actualModel.AuthorId);
            Assert.Equal(this.testPost.Author.UserName, actualModel.AuthorUserName);
            Assert.Equal(this.testPost.Author.ProfilePictureURL, actualModel.AuthorProfilePictureURL);
            Assert.Equal(TestCategoryPostId, actualModel.Category.Id);
            Assert.Equal(this.testPost.Category.Name, actualModel.Category.Name);
        }

        [Fact]
        public async Task GetPostDetailsViewModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            PostDetailsViewModel? actualModel = await service.GetPostDetailsViewModelAsync(TestPostForActionId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task GetPostInputModelAsync_ShouldReturnExpectedPostInputModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Categories.AddAsync(new Category { Id = 1000, Name = "Test Category Name", Description = "Test Category Description", ImageUrl = "ImageUrl" });
            await dbContext.Categories.AddAsync(new Category { Id = 1001, Name = "Test Other Category Name", Description = "Test Other Category Description", ImageUrl = "ImageUrl" });

            await dbContext.Tags.AddAsync(new Tag { Id = 1010, Name = "Test Tag Name" });
            await dbContext.Tags.AddAsync(new Tag { Id = 1011, Name = "Test Other Tag Name" });
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            PostInputModel actualModel = await service.GetPostInputModelAsync();

            Assert.NotNull(actualModel);
            Assert.Equal(2, actualModel.Categories.Count());
            Assert.Equal(2, actualModel.Tags.Count());
        }

        [Fact]
        public async Task CreateAsync_ShouldCreatePost()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            int actualPostId = await service.CreateAsync(TestPostTitle, TestPostContent, "ImageUrl", TestPostAuthorId, TestCategoryPostId, new List<int>());
            Post? post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Title == TestPostTitle);

            Assert.NotNull(post);
            Assert.Equal(TestPostTitle, post.Title);
            Assert.Equal(TestPostContent, post.Content);
            Assert.Equal(TestPostAuthorId, post.AuthorId);
            Assert.Equal(TestCategoryPostId, post.CategoryId);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedPostId()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            int actualPostId = await service.CreateAsync(TestPostTitle, TestPostContent, "ImageUrl", TestPostAuthorId, TestCategoryPostId, new List<int>());
            Post? post = await dbContext.Posts.FindAsync(actualPostId);

            Assert.NotNull(post);
            Assert.Equal(TestPostTitle, post.Title);
            Assert.Equal(TestPostContent, post.Content);
            Assert.Equal(TestPostAuthorId, post.AuthorId);
            Assert.Equal(TestCategoryPostId, post.CategoryId);
        }

        [Fact]
        public async Task GetAuthorIdAsync_ShouldReturnExpectedAuthorId()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            string? actualAuthorId = await service.GetAuthorIdAsync(TestPostId);

            Assert.NotNull(actualAuthorId);
            Assert.Equal(TestPostAuthorId, actualAuthorId);
        }

        [Fact]
        public async Task GetAuthorIdAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            string? actualAuthorId = await service.GetAuthorIdAsync(TestPostForActionId);

            Assert.Null(actualAuthorId);
        }

        [Fact]
        public async Task GetPostEditModelAsync_ShouldReturnExpectedPostEditModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            PostEditModel? actualModel = await service.GetPostEditModelAsync(TestPostId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestPostTitle, actualModel.Title);
            Assert.Equal(TestPostContent, actualModel.Content);
        }

        [Fact]
        public async Task GetPostEditModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            PostEditModel? actualModel = await service.GetPostEditModelAsync(TestPostForActionId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task EditAsync_ShouldEdit()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            bool isEdited = await service.EditAsync(TestPostForActionId, TestPostTitle, TestOtherPostContent, this.testPostForAction.ImageUrl, TestCategoryPostId, new List<int>());
            Post? post = await dbContext.Posts.FindAsync(TestPostForActionId);

            Assert.NotNull(post);
            Assert.Equal(TestPostForActionId, post.Id);
            Assert.Equal(TestPostTitle, post.Title);
            Assert.Equal(TestOtherPostContent, post.Content);
            Assert.Equal(TestCategoryPostId, post.CategoryId);
        }

        [Fact]
        public async Task EditAsync_ShouldReturnTrue_IfEdited()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            bool isEdited = await service.EditAsync(TestPostForActionId, TestPostTitle, TestOtherPostContent, this.testPostForAction.ImageUrl, TestCategoryPostId, new List<int>());
            Post? post = await dbContext.Posts.FindAsync(TestPostForActionId);

            Assert.True(isEdited);
            Assert.NotNull(post);
            Assert.Equal(TestPostForActionId, post.Id);
            Assert.Equal(TestPostTitle, post.Title);
            Assert.Equal(TestOtherPostContent, post.Content);
            Assert.Equal(TestCategoryPostId, post.CategoryId);
        }

        [Fact]
        public async Task EditAsync_ShouldReturnFalse_IfNotEdited()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            bool isEdited = await service.EditAsync(TestPostForActionId, TestPostTitle, TestOtherPostContent, this.testPostForAction.ImageUrl, TestCategoryPostId, new List<int>());
            Post? post = await dbContext.Posts.FindAsync(TestPostForActionId);

            Assert.False(isEdited);
            Assert.Null(post);
        }

        [Fact]
        public async Task GetPostDeleteModelAsync_ShouldReturnExpectedPostDeleteModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            PostDeleteModel? actualModel = await service.GetPostDeleteModelAsync(TestPostId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestPostId, actualModel.Id);
            Assert.Equal(TestPostTitle, actualModel.Title);
            Assert.Equal(TestPostContent, actualModel.Content);
            Assert.Equal(0, actualModel.CommentsCount);
            Assert.Equal(0, actualModel.VotesCount);
            Assert.Equal(this.testPost.Author.UserName, actualModel.AuthorUserName);
            Assert.Equal(this.testPost.Author.ProfilePictureURL, actualModel.AuthorProfilePictureURL);
            Assert.Equal(TestCategoryPostId, actualModel.Category.Id);
            Assert.Equal(this.testPost.Category.Name, actualModel.Category.Name);
        }

        [Fact]
        public async Task GetPostDeleteModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            PostDeleteModel? actualModel = await service.GetPostDeleteModelAsync(TestPostForActionId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            bool isDeleted = await service.DeleteAsync(TestPostForActionId);
            Post? post = await dbContext.Posts.FindAsync(TestPostForActionId);

            Assert.NotNull(post);
            Assert.True(post.IsDeleted);
            Assert.Equal(TestPostForActionId, post.Id);            
            Assert.Equal(TestPostTitle, post.Title);            
            Assert.Equal(TestPostContent, post.Content);            
            Assert.Equal(TestPostAuthorId, post.AuthorId);            
            Assert.Equal(TestCategoryPostId, post.CategoryId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_IfDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Posts.AddAsync(this.testPostForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            bool isDeleted = await service.DeleteAsync(TestPostForActionId);
            Post? post = await dbContext.Posts.FindAsync(TestPostForActionId);

            Assert.True(isDeleted);
            Assert.NotNull(post);
            Assert.True(post.IsDeleted);
            Assert.Equal(TestPostForActionId, post.Id);
            Assert.Equal(TestPostTitle, post.Title);
            Assert.Equal(TestPostContent, post.Content);
            Assert.Equal(TestPostAuthorId, post.AuthorId);
            Assert.Equal(TestCategoryPostId, post.CategoryId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_IfNotDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Category> categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            using IDeletableEntityRepository<Tag> tagsRepository = new EfDeletableEntityRepository<Tag>(dbContext);
            using IDeletableEntityRepository<PostTag> postsTagsRepository = new EfDeletableEntityRepository<PostTag>(dbContext);
            PostsService service = new PostsService(postsRepository, categoriesRepository, tagsRepository, postsTagsRepository);

            bool isDeleted = await service.DeleteAsync(TestPostForActionId);
            Post? post = await dbContext.Posts.FindAsync(TestPostForActionId);

            Assert.False(isDeleted);
            Assert.Null(post);
            Assert.Equal(2, dbContext.Posts.Count());
        }
    }
}