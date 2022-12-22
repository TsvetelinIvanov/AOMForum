using AOMForum.Data.Common.Repositories;
using AOMForum.Data;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using AOMForum.Data.Repositories;
using AOMForum.Services.Data.Services;
using AOMForum.Web.Models.CommentReports;
using Microsoft.EntityFrameworkCore;
using AOMForum.Web.Models.PostReports;

namespace AOMForum.Services.Data.Tests
{
    public class PostReportsServiceTests
    {
        private const int TestPostReportId = 10;
        private const int TestOtherPostReportId = 11;
        private const int TestInexistantPostReportId = 101;
        private const int TestPostId = 100;
        private const int TestInexistantPostId = 110;
        private const int TestCategoryPostId = 1000;
        private const string TestPostReportAuthorId = "TestPostReportAuthorId";
        private const string TestOtherPostReportAuthorId = "TestOtherPostReportAuthorId";
        private const string TestPostAuthorId = "TestPostAuthorId";
        private const string TestPostReportContent = "Test Post Report Content";
        private const string TestOtherPostReportContent = "Test Other Post Report Content";

        private readonly Post testPost = new Post()
        {
            Id = TestPostId,
            Title = "Test Post Title",
            Type = PostType.Text,
            ImageUrl = "TestPostImageUrl",
            Content = "Test Post Content",
            AuthorId = TestPostAuthorId,
            Author = new ApplicationUser()
            {
                Id = TestPostAuthorId,
                UserName = "TestAuthor",
                Email = "testauthor@mail.com",
                FirstName = "Test",
                SecondName = "Post",
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

        private readonly PostReport testPostReport = new PostReport()
        {
            Id = TestPostReportId,
            Content = TestPostReportContent,
            AuthorId = TestPostReportAuthorId,
            Author = new ApplicationUser()
            {
                Id = TestPostReportAuthorId,
                UserName = "TestPostReportAuthor",
                Email = "testpostreportauthor@mail.com",
                FirstName = "Test",
                SecondName = "PostReport",
                LastName = "Author",
                BirthDate = new DateTime(1999, 1, 1),
                Age = 23,
                Gender = GenderType.Male,
                Biography = "Test Post Report Author Biography",
                ProfilePictureURL = "ProfilePictureURL",
                EmailConfirmed = true
            },
            PostId = TestPostId
        };

        private readonly PostReport testOtherPostReport = new PostReport()
        {
            Id = TestOtherPostReportId,
            Content = TestOtherPostReportContent,
            AuthorId = TestOtherPostReportAuthorId,
            Author = new ApplicationUser()
            {
                Id = TestOtherPostReportAuthorId,
                UserName = "TestOtherPostReportAuthor",
                Email = "testpostreportauthor@mail.com",
                FirstName = "TestOther",
                SecondName = "PostReport",
                LastName = "Author",
                BirthDate = new DateTime(1999, 1, 1),
                Age = 23,
                Gender = GenderType.Female,
                Biography = "Test Other Post Report Author Biography",
                ProfilePictureURL = "ProfilePictureURL",
                EmailConfirmed = true
            },
            PostId = TestPostId
        };

        [Fact]
        public async Task GetCommentReportListViewModelsAsync_ShouldReturnExpectedCommentReportListViewModels()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            await dbContext.PostReports.AddAsync(this.testPostReport);
            await dbContext.PostReports.AddAsync(this.testOtherPostReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            IEnumerable<PostReportListViewModel> actualModels = await service.GetPostReportListViewModelsAsync();

            Assert.NotNull(actualModels);
            Assert.Equal(2, actualModels.Count());
            foreach (PostReportListViewModel actualModel in actualModels)
            {
                Assert.Contains(actualModels, pr => pr.Id == TestPostReportId);
                Assert.Contains(actualModels, pr => pr.Id == TestOtherPostReportId);
            }
        }

        [Fact]
        public async Task GetPostReportListViewModelsAsync_ShouldReturnEmptyCollection_IfNoPostReports()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            IEnumerable<PostReportListViewModel> actualModels = await service.GetPostReportListViewModelsAsync();

            Assert.NotNull(actualModels);
            Assert.IsAssignableFrom<IEnumerable<PostReportListViewModel>>(actualModels);
            Assert.Empty(actualModels);
        }

        [Fact]
        public async Task GetPostReportDetailsViewModelAsync_ShouldReturnExpectedPostReportDetailsViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            await dbContext.PostReports.AddAsync(this.testPostReport);
            await dbContext.PostReports.AddAsync(this.testOtherPostReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            PostReportDetailsViewModel? actualModel = await service.GetPostReportDetailsViewModelAsync(TestPostReportId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestPostReportId, actualModel.Id);
            Assert.Equal(this.testPostReport.Content, actualModel.Content);
            Assert.Equal(this.testPostReport.PostId, actualModel.PostId);
            Assert.Equal(this.testPost.Title, actualModel.PostTitle);
            Assert.Equal(this.testPostReport.AuthorId, actualModel.AuthorId);
            Assert.Equal(this.testPostReport.Author.UserName, actualModel.AuthorUserName);
            Assert.Equal(this.testPostReport.Author.ProfilePictureURL, actualModel.AuthorProfilePictureURL);
        }

        [Fact]
        public async Task GetPostReportDetailsViewModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            await dbContext.PostReports.AddAsync(this.testPostReport);
            await dbContext.PostReports.AddAsync(this.testOtherPostReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            PostReportDetailsViewModel? actualModel = await service.GetPostReportDetailsViewModelAsync(TestInexistantPostReportId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task GetPostReportInputModelAsync_ShouldReturnExpectedPostReportInputModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            PostReportInputModel? actualModel = await service.GetPostReportInputModelAsync(TestPostId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestPostId, actualModel.PostId);
        }

        [Fact]
        public async Task GetPostReportInputModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            PostReportInputModel? actualModel = await service.GetPostReportInputModelAsync(TestInexistantPostId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreatePostReport()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            bool isCreated = await service.CreateAsync(TestPostReportContent, TestPostId, TestPostReportAuthorId);
            PostReport? postReport = await dbContext.PostReports.FirstOrDefaultAsync(p => p.Content == TestPostReportContent);

            Assert.NotNull(postReport);
            Assert.Equal(TestPostReportAuthorId, postReport.AuthorId);
            Assert.Equal(TestPostId, postReport.PostId);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnTrue_IfCreated()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            bool isCreated = await service.CreateAsync(TestPostReportContent, TestPostId, TestPostReportAuthorId);
            PostReport? postReport = await dbContext.PostReports.FirstOrDefaultAsync(p => p.Content == TestPostReportContent);

            Assert.True(isCreated);
            Assert.NotNull(postReport);
            Assert.Equal(TestPostReportAuthorId, postReport.AuthorId);
            Assert.Equal(TestPostId, postReport.PostId);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFalse_IfNotCreated()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            bool isCreated = await service.CreateAsync(TestPostReportContent, TestPostId, TestPostReportAuthorId);
            PostReport? postReport = await dbContext.PostReports.FirstOrDefaultAsync(p => p.Content == TestPostReportContent);

            Assert.False(isCreated);
            Assert.Null(postReport);
        }

        [Fact]
        public async Task GetPostReportDeleteModelAsync_ShouldReturnExpectedPostReportDeleteModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            await dbContext.PostReports.AddAsync(this.testPostReport);
            await dbContext.PostReports.AddAsync(this.testOtherPostReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            PostReportDeleteModel? actualModel = await service.GetPostReportDeleteModelAsync(TestPostReportId);

            Assert.NotNull(actualModel);
            Assert.IsType<PostReportDeleteModel>(actualModel);
            Assert.Equal(TestPostReportId, actualModel.Id);
            Assert.Equal(this.testPostReport.Content, actualModel.Content);
            Assert.Equal(this.testPostReport.PostId, actualModel.PostId);
            Assert.Equal(this.testPost.Title, actualModel.PostTitle);
            Assert.Equal(this.testPostReport.Author.UserName, actualModel.AuthorUserName);
            Assert.Equal(this.testPostReport.Author.ProfilePictureURL, actualModel.AuthorProfilePictureURL);
        }

        [Fact]
        public async Task GetPostReportDeleteModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            await dbContext.PostReports.AddAsync(this.testPostReport);
            await dbContext.PostReports.AddAsync(this.testOtherPostReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            PostReportDeleteModel? actualModel = await service.GetPostReportDeleteModelAsync(TestInexistantPostReportId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            await dbContext.PostReports.AddAsync(this.testPostReport);
            await dbContext.PostReports.AddAsync(this.testOtherPostReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            bool isDeleted = await service.DeleteAsync(TestPostReportId);
            PostReport? postReport = await dbContext.PostReports.FindAsync(TestPostReportId);

            Assert.NotNull(postReport);
            Assert.True(postReport.IsDeleted);
            Assert.Equal(TestPostReportId, postReport.Id);
            Assert.Equal(TestPostReportAuthorId, postReport.AuthorId);
            Assert.Equal(TestPostId, postReport.PostId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_IfDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            await dbContext.PostReports.AddAsync(this.testPostReport);
            await dbContext.PostReports.AddAsync(this.testOtherPostReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            bool isDeleted = await service.DeleteAsync(TestPostReportId);
            PostReport? postReport = await dbContext.PostReports.FindAsync(TestPostReportId);

            Assert.True(isDeleted);
            Assert.NotNull(postReport);
            Assert.True(postReport.IsDeleted);
            Assert.Equal(TestPostReportId, postReport.Id);
            Assert.Equal(TestPostReportAuthorId, postReport.AuthorId);
            Assert.Equal(TestPostId, postReport.PostId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_IfNotDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.SaveChangesAsync();

            await dbContext.PostReports.AddAsync(this.testPostReport);
            await dbContext.PostReports.AddAsync(this.testOtherPostReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostReport> postReportsRepository = new EfDeletableEntityRepository<PostReport>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            PostReportsService service = new PostReportsService(postReportsRepository, postsRepository);

            bool isDeleted = await service.DeleteAsync(TestInexistantPostReportId);
            PostReport? postReport = await dbContext.PostReports.FindAsync(TestInexistantPostReportId);

            Assert.False(isDeleted);
            Assert.Null(postReport);
        }
    }
}