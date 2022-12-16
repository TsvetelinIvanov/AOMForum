using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Repositories;
using AOMForum.Data;
using AOMForum.Services.Data.Services;
using AOMForum.Web.Models.CommentReports;
using Microsoft.EntityFrameworkCore;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;

namespace AOMForum.Services.Data.Tests
{
    public class CommentReportsServiceTests
    {
        private const int TestCommentReportId = 10;
        private const int TestOtherCommentReportId = 11;
        private const int TestInexistantCommentReportId = 101;
        private const int TestCommentId = 100;
        private const int TestInexistantCommentId = 1001;
        private const string TestCommentReportAuthorId = "TestCommentReportAuthorId";
        private const string OtherTestCommentReportAuthorId = "OtherTestCommentReportAuthorId";
        private const string TestCommentAuthorId = "TestCommentAuthorId";
        private const string TestCommentPostAuthorId = "TestCommentPostAuthorId";
        private const int TestPostCommentId = 1000;
        private const int TestCategoryPostCommentId = 10000;
        private const string TestCommentReportContent = "Test Comment Report Content";
        private const string TestOtherCommentReportContent = "Test Other Comment Report Content";

        private readonly Comment testComment = new Comment()
        {
            Id = TestCommentId,
            Content = "Test Comment Content",
            AuthorId = TestCommentAuthorId,
            Author = new ApplicationUser()
            {
                Id = TestCommentAuthorId,
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
            PostId = TestPostCommentId,
            Post = new Post()
            {
                Id = TestCategoryPostCommentId,
                Title = "Test Post Title",
                Type = PostType.Text,
                ImageUrl = "TestPostImageUrl",
                Content = "Test Post Content",
                AuthorId = TestCommentPostAuthorId,
                CategoryId = TestCategoryPostCommentId
            }
        };

        private readonly CommentReport testCommentReport = new CommentReport()
        {
            Id = TestCommentReportId,
            Content = TestCommentReportContent,
            AuthorId = TestCommentReportAuthorId,
            Author = new ApplicationUser()
            {
                Id = TestCommentReportAuthorId,
                UserName = "TestCommentReportAuthor",
                Email = "testcommentreportauthor@mail.com",
                FirstName = "Test",
                SecondName = "CommentReport",
                LastName = "Author",
                BirthDate = new DateTime(1999, 1, 1),
                Age = 23,
                Gender = GenderType.Male,
                Biography = "Test Comment Report Author Biography",
                ProfilePictureURL = "ProfilePictureURL",
                EmailConfirmed = true
            },
            CommentId = TestCommentId,
            CreatedOn = DateTime.UtcNow,
            IsDeleted = false
        };

        private readonly CommentReport testOtherCommentReport = new CommentReport()
        {
            Id = TestOtherCommentReportId,
            Content = TestOtherCommentReportContent,
            AuthorId = OtherTestCommentReportAuthorId,
            Author = new ApplicationUser()
            {
                Id = OtherTestCommentReportAuthorId,
                UserName = "OtherTestCommentReportAuthor",
                Email = "othertestcommentreportauthor@mail.com",
                FirstName = "Other",
                SecondName = "TestCommentReport",
                LastName = "Author",
                BirthDate = new DateTime(1999, 1, 1),
                Age = 23,
                Gender = GenderType.Female,
                Biography = "Other Test Comment Report Author Biography",
                ProfilePictureURL = "OtherProfilePictureURL",
                EmailConfirmed = true
            },
            CommentId = TestCommentId,
            CreatedOn = DateTime.UtcNow,
            IsDeleted = false
        };

        [Fact]
        public async Task GetCommentReportListViewModelsAsync_ShouldReturnExpectedCommentReportListViewModels()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();

            await dbContext.CommentReports.AddAsync(this.testCommentReport);
            await dbContext.CommentReports.AddAsync(this.testOtherCommentReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            IEnumerable<CommentReportListViewModel> actualModels = await service.GetCommentReportListViewModelsAsync();

            Assert.NotNull(actualModels);
            Assert.IsAssignableFrom<IEnumerable<CommentReportListViewModel>>(actualModels);
            Assert.Equal(2, actualModels.Count());
            foreach (CommentReportListViewModel actualModel in actualModels)
            {
                Assert.Contains(actualModels, cr => cr.Id == TestCommentReportId);
                Assert.Contains(actualModels, cr => cr.Id == TestOtherCommentReportId);
            }
        }

        [Fact]
        public async Task GetCommentReportListViewModelsAsync_ShouldReturnEmptyCollection_IfNoCommentReports()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            IEnumerable<CommentReportListViewModel> actualModels = await service.GetCommentReportListViewModelsAsync();

            Assert.NotNull(actualModels);
            Assert.IsAssignableFrom<IEnumerable<CommentReportListViewModel>>(actualModels);
            Assert.Empty(actualModels);
        }

        [Fact]
        public async Task GetCommentReportDetailsViewModelAsync_ShouldReturnExpectedCommentReportDetailsViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();

            await dbContext.CommentReports.AddAsync(this.testCommentReport);
            await dbContext.CommentReports.AddAsync(this.testOtherCommentReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            CommentReportDetailsViewModel? actualModel = await service.GetCommentReportDetailsViewModelAsync(TestCommentReportId);

            Assert.NotNull(actualModel);
            Assert.IsType<CommentReportDetailsViewModel>(actualModel);
            Assert.Equal(TestCommentReportId, actualModel.Id);
            Assert.Equal(this.testCommentReport.Content, actualModel.Content);
            Assert.Equal(this.testCommentReport.CommentId, actualModel.CommentId);
            Assert.Equal(this.testComment.Content, actualModel.CommentContent);
            Assert.Equal(this.testCommentReport.AuthorId, actualModel.AuthorId);
            Assert.Equal(this.testCommentReport.Author.UserName, actualModel.AuthorUserName);
            Assert.Equal(this.testCommentReport.Author.ProfilePictureURL, actualModel.AuthorProfilePictureURL);
        }

        [Fact]
        public async Task GetCommentReportDetailsViewModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();

            await dbContext.CommentReports.AddAsync(this.testCommentReport);
            await dbContext.CommentReports.AddAsync(this.testOtherCommentReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            CommentReportDetailsViewModel? actualModel = await service.GetCommentReportDetailsViewModelAsync(TestInexistantCommentReportId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task GetCommentReportInputModelAsync_ShouldReturnExpectedCommentReportInputModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();            

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            CommentReportInputModel? actualModel = await service.GetCommentReportInputModelAsync(TestCommentId);

            Assert.NotNull(actualModel);
            Assert.IsType<CommentReportInputModel>(actualModel);
            Assert.Equal(TestCommentId, actualModel.CommentId);
        }

        [Fact]
        public async Task GetCommentReportInputModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            CommentReportInputModel? actualModel = await service.GetCommentReportInputModelAsync(TestInexistantCommentId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateCommentReport()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            bool isCreated = await service.CreateAsync(TestCommentReportContent, TestCommentId, TestCommentReportAuthorId);
            CommentReport? commentReport = await dbContext.CommentReports.FirstOrDefaultAsync(c => c.Content == TestCommentReportContent);

            Assert.NotNull(commentReport);
            Assert.Equal(TestCommentReportId, commentReport.Id);
            Assert.Equal(TestCommentReportAuthorId, commentReport.AuthorId);
            Assert.Equal(TestCommentId, commentReport.CommentId);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnTrue_IfCreated()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            bool isCreated = await service.CreateAsync(TestCommentReportContent, TestCommentId, TestCommentReportAuthorId);
            CommentReport? commentReport = await dbContext.CommentReports.FirstOrDefaultAsync(c => c.Content == TestCommentReportContent);

            Assert.True(isCreated);
            Assert.NotNull(commentReport);
            Assert.Equal(TestCommentReportId, commentReport.Id);
            Assert.Equal(TestCommentReportAuthorId, commentReport.AuthorId);
            Assert.Equal(TestCommentId, commentReport.CommentId);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFalse_IfNotCreated()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);            

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            bool isCreated = await service.CreateAsync(TestCommentReportContent, TestCommentId, TestCommentReportAuthorId);
            CommentReport? commentReport = await dbContext.CommentReports.FirstOrDefaultAsync(c => c.Content == TestCommentReportContent);

            Assert.False(isCreated);
            Assert.Null(commentReport);
        }

        [Fact]
        public async Task GetCommentReportDeleteModelAsync_ShouldReturnExpectedCommentReportDeleteModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();

            await dbContext.CommentReports.AddAsync(this.testCommentReport);
            await dbContext.CommentReports.AddAsync(this.testOtherCommentReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            CommentReportDeleteModel? actualModel = await service.GetCommentReportDeleteModelAsync(TestCommentReportId);

            Assert.NotNull(actualModel);
            Assert.IsType<CommentReportDeleteModel>(actualModel);
            Assert.Equal(TestCommentReportId, actualModel.Id);
            Assert.Equal(this.testCommentReport.Content, actualModel.Content);
            Assert.Equal(this.testCommentReport.CommentId, actualModel.CommentId);
            Assert.Equal(this.testComment.Content, actualModel.CommentContent);
            Assert.Equal(this.testCommentReport.Author.UserName, actualModel.AuthorUserName);
            Assert.Equal(this.testCommentReport.Author.ProfilePictureURL, actualModel.AuthorProfilePictureURL);
        }

        [Fact]
        public async Task GetCommentReportDeleteModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();

            await dbContext.CommentReports.AddAsync(this.testCommentReport);
            await dbContext.CommentReports.AddAsync(this.testOtherCommentReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            CommentReportDeleteModel? actualModel = await service.GetCommentReportDeleteModelAsync(TestInexistantCommentReportId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();

            await dbContext.CommentReports.AddAsync(this.testCommentReport);
            await dbContext.CommentReports.AddAsync(this.testOtherCommentReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            bool isDeleted = await service.DeleteAsync(TestCommentReportId);
            CommentReport? commentReport = await dbContext.CommentReports.FindAsync(TestCommentReportId);

            Assert.NotNull(commentReport);
            Assert.True(commentReport.IsDeleted);
            Assert.Equal(TestCommentReportId, commentReport.Id);
            Assert.Equal(TestCommentReportAuthorId, commentReport.AuthorId);
            Assert.Equal(TestCommentId, commentReport.CommentId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_IfDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();

            await dbContext.CommentReports.AddAsync(this.testCommentReport);
            await dbContext.CommentReports.AddAsync(this.testOtherCommentReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            bool isDeleted = await service.DeleteAsync(TestCommentReportId);
            CommentReport? commentReport = await dbContext.CommentReports.FindAsync(TestCommentReportId);

            Assert.True(isDeleted);
            Assert.NotNull(commentReport);
            Assert.True(commentReport.IsDeleted);
            Assert.Equal(TestCommentReportId, commentReport.Id);
            Assert.Equal(TestCommentReportAuthorId, commentReport.AuthorId);
            Assert.Equal(TestCommentId, commentReport.CommentId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_IfNotDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.SaveChangesAsync();

            await dbContext.CommentReports.AddAsync(this.testCommentReport);
            await dbContext.CommentReports.AddAsync(this.testOtherCommentReport);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentReport> commentReportsRepository = new EfDeletableEntityRepository<CommentReport>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentReportsService service = new CommentReportsService(commentReportsRepository, commentsRepository);

            bool isDeleted = await service.DeleteAsync(TestCommentReportId);
            CommentReport? commentReport = await dbContext.CommentReports.FindAsync(TestInexistantCommentReportId);
            
            Assert.False(isDeleted);
            Assert.Null(commentReport);
        }
    }
}