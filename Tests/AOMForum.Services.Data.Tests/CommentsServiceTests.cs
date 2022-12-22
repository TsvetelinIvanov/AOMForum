using AOMForum.Data.Common.Repositories;
using AOMForum.Data;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using AOMForum.Data.Repositories;
using AOMForum.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using AOMForum.Web.Models.Comments;

namespace AOMForum.Services.Data.Tests
{
    public class CommentsServiceTests
    {
        private const int TestCommentId = 10;
        private const int TestOtherCommentId = 11;
        private const int TestCommentForActionId = 101;
        private const int TestPostId = 100;
        private const int TestOtherPostId = 110;
        private const int TestLastPostId = 111;
        private const int TestCategoryPostId = 1000;
        private const string TestCommentAuthorId = "TestCommentAuthorId";
        private const string TestOtherCommentAuthorId = "TestOtherCommentAuthorId";
        private const string TestLastCommentAuthorId = "TestLastCommentAuthorId";
        private const string TestPostAuthorId = "TestCommentPostAuthorId";
        private const string TestCommentContent = "Test Comment Content";
        private const string TestOtherCommentContent = "Test Other Comment Content";

        private readonly Comment testComment = new Comment()
        {
            Id = TestCommentId,
            Content = TestCommentContent,
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
            PostId = TestPostId,
            Post = new Post()
            {
                Id = TestPostId,
                Title = "Test Post Title",
                Type = PostType.Text,
                ImageUrl = "TestPostImageUrl",
                Content = "Test Post Content",
                AuthorId = TestPostAuthorId,
                CategoryId = TestCategoryPostId
            }
        };

        private readonly Comment testOtherComment = new Comment()
        {
            Id = TestOtherCommentId,
            Content = TestOtherCommentContent,
            AuthorId = TestOtherCommentAuthorId,
            Author = new ApplicationUser()
            {
                Id = TestOtherCommentAuthorId,
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
            PostId = TestOtherPostId,
            Post = new Post()
            {
                Id = TestOtherPostId,
                Title = "Test Post Title",
                Type = PostType.Text,
                ImageUrl = "TestPostImageUrl",
                Content = "Test Post Content",
                AuthorId = TestPostAuthorId,
                CategoryId = TestCategoryPostId
            }
        };

        private readonly Comment testCommentForAction = new Comment()
        {
            Id = TestCommentForActionId,
            Content = TestCommentContent,
            AuthorId = TestLastCommentAuthorId,
            Author = new ApplicationUser()
            {
                Id = TestLastCommentAuthorId,
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
            PostId = TestLastPostId,
            Post = new Post()
            {
                Id = TestLastPostId,
                Title = "Test Post Title",
                Type = PostType.Text,
                ImageUrl = "TestPostImageUrl",
                Content = "Test Post Content",
                AuthorId = TestPostAuthorId,
                CategoryId = TestCategoryPostId
            }
        };

        [Fact]
        public async Task GetCommentDetailsViewModelAsync_ShouldReturnExpectedCommentDetailsViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            CommentDetailsViewModel? actualModel = await service.GetCommentDetailsViewModelAsync(TestCommentId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestCommentId, actualModel.Id);
            Assert.Equal(TestCommentContent, actualModel.Content);
            Assert.Equal(TestCommentAuthorId, actualModel.AuthorId);
            Assert.Equal(this.testComment.Author.UserName, actualModel.AuthorUserName);
            Assert.Equal(this.testComment.Author.ProfilePictureURL, actualModel.AuthorProfilePictureURL);
            Assert.Equal(TestPostId, actualModel.PostId);
            Assert.Equal(TestPostAuthorId, actualModel.PostAuthorId);
        }

        [Fact]
        public async Task GetCommentDetailsViewModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            CommentDetailsViewModel? actualModel = await service.GetCommentDetailsViewModelAsync(TestCommentForActionId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateComment()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            int actualCommentId = await service.CreateAsync(TestCommentContent, /*null, */TestPostId, TestCommentAuthorId);
            Comment? comment = await dbContext.Comments.FirstOrDefaultAsync(c => c.Content == TestCommentContent);

            Assert.NotNull(comment);
            Assert.Equal(TestPostId, comment.PostId);
            Assert.Equal(TestCommentAuthorId, comment.AuthorId);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedCategoryId()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            int actualCommentId = await service.CreateAsync(TestCommentContent, /*null, */TestPostId, TestCommentAuthorId);
            Comment? comment = await dbContext.Comments.FindAsync(actualCommentId);

            Assert.NotNull(comment);
            Assert.Equal(TestCommentContent, comment.Content);
            Assert.Equal(TestPostId, comment.PostId);
            Assert.Equal(TestCommentAuthorId, comment.AuthorId);
        }

        [Fact]
        public async Task GetAuthorIdAsync_ShouldReturnExpectedAuthorId()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            string? actualAuthorId = await service.GetAuthorIdAsync(TestCommentId);

            Assert.NotNull(actualAuthorId);
            Assert.Equal(TestCommentAuthorId, actualAuthorId);
        }

        [Fact]
        public async Task GetAuthorIdAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            string? actualAuthorId = await service.GetAuthorIdAsync(TestCommentForActionId);

            Assert.Null(actualAuthorId);
        }

        [Fact]
        public async Task GetCommentEditModelAsync_ShouldReturnExpectedCommentEditModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            CommentEditModel? actualModel = await service.GetCommentEditModelAsync(TestCommentId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestCommentContent, actualModel.Content);
        }

        [Fact]
        public async Task GetCommentEditModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            CommentEditModel? actualModel = await service.GetCommentEditModelAsync(TestCommentForActionId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task EditAsync_ShouldEdit()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.Comments.AddAsync(this.testCommentForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            bool isEdited = await service.EditAsync(TestCommentForActionId, TestOtherCommentContent);
            Comment? comment = await dbContext.Comments.FindAsync(TestCommentForActionId);

            Assert.NotNull(comment);
            Assert.Equal(TestCommentForActionId, comment.Id);
            Assert.Equal(TestOtherCommentContent, comment.Content);
        }

        [Fact]
        public async Task EditAsync_ShouldReturnTrue_IfEdited()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.Comments.AddAsync(this.testCommentForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            bool isEdited = await service.EditAsync(TestCommentForActionId, TestOtherCommentContent);
            Comment? comment = await dbContext.Comments.FindAsync(TestCommentForActionId);

            Assert.True(isEdited);
            Assert.NotNull(comment);
            Assert.Equal(TestCommentForActionId, comment.Id);
            Assert.Equal(TestOtherCommentContent, comment.Content);
        }

        [Fact]
        public async Task EditAsync_ShouldReturnFalse_IfNotEdited()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            bool isEdited = await service.EditAsync(TestCommentForActionId, TestOtherCommentContent);
            Comment? comment = await dbContext.Comments.FindAsync(TestCommentForActionId);

            Assert.False(isEdited);
            Assert.Null(comment);
        }

        [Fact]
        public async Task GetCommentDeleteModelAsync_ShouldReturnExpectedCommentDeleteModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            CommentDeleteModel? actualModel = await service.GetCommentDeleteModelAsync(TestCommentId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestCommentContent, actualModel.Content);
            Assert.Equal(this.testComment.Author.UserName, actualModel.AuthorUserName);
            Assert.Equal(this.testComment.Author.ProfilePictureURL, actualModel.AuthorProfilePictureURL);
            Assert.Equal(TestPostId, actualModel.PostId);
            Assert.Equal(this.testComment.Post.Title, actualModel.PostTitle);
        }

        [Fact]
        public async Task GetCommentDeleteModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            CommentDeleteModel? actualModel = await service.GetCommentDeleteModelAsync(TestCommentForActionId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.Comments.AddAsync(this.testCommentForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            bool isDeleted = await service.DeleteAsync(TestCommentForActionId);
            Comment? comment = await dbContext.Comments.FindAsync(TestCommentForActionId);

            Assert.NotNull(comment);
            Assert.True(comment.IsDeleted);
            Assert.Equal(TestCommentForActionId, comment.Id);
            Assert.Equal(TestCommentContent, comment.Content);
            Assert.Equal(this.testComment.Author.UserName, comment.Author.UserName);
            Assert.Equal(this.testComment.Author.ProfilePictureURL, comment.Author.ProfilePictureURL);
            Assert.Equal(TestLastPostId, comment.PostId);
            Assert.Equal(this.testComment.Post.Title, comment.Post.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_IfDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.Comments.AddAsync(this.testCommentForAction);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            bool isDeleted = await service.DeleteAsync(TestCommentForActionId);
            Comment? comment = await dbContext.Comments.FindAsync(TestCommentForActionId);

            Assert.True(isDeleted);
            Assert.NotNull(comment);
            Assert.True(comment.IsDeleted);
            Assert.Equal(TestCommentForActionId, comment.Id);
            Assert.Equal(TestCommentContent, comment.Content);
            Assert.Equal(this.testComment.Author.UserName, comment.Author.UserName);
            Assert.Equal(this.testComment.Author.ProfilePictureURL, comment.Author.ProfilePictureURL);
            Assert.Equal(TestLastPostId, comment.PostId);
            Assert.Equal(this.testComment.Post.Title, comment.Post.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_IfNotDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Comment> repository = new EfDeletableEntityRepository<Comment>(dbContext);
            CommentsService service = new CommentsService(repository);

            bool isDeleted = await service.DeleteAsync(TestCommentForActionId);
            Comment? comment = await dbContext.Comments.FindAsync(TestCommentForActionId);

            Assert.False(isDeleted);
            Assert.Null(comment);
            Assert.Equal(2, dbContext.Comments.Count());
        }
    }
}