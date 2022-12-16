using AOMForum.Data.Common.Repositories;
using AOMForum.Data;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using AOMForum.Data.Repositories;
using AOMForum.Services.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace AOMForum.Services.Data.Tests
{
    public class CommentVotesServiceTests
    {
        private const int TestVoteId = 10;
        private const int TestOtherVoteId = 11;
        private const int TestCommentId = 100;        
        private const int TestCommentForActionId = 101;
        private const int TestPostId = 1000;
        private const int TestCategoryPostId = 10000;
        private const string TestVotedUserId = "TestVotedUserId";
        private const string TestOtherVotedUserId = "TestOtherVotedUserId";
        private const string TestCommentAuthorId = "TestCommentAuthorId";
        private const string TestOtherCommentAuthorId = "OtherTestCommentAuthorId";
        private const string TestPostAuthorId = "TestCommentPostAuthorId";
        private const string TestCommentContent = "Test Comment Content";
        private const string TestOtherCommentContent = "Test Other Comment Content";

        private readonly CommentVote testCommentVote = new CommentVote()
        {
            Id = TestVoteId,
            Type = VoteType.UpVote,
            CommentId = TestCommentId,
            AuthorId = TestVotedUserId
        };

        private readonly CommentVote testOtherCommentVote = new CommentVote()
        {
            Id = TestOtherVoteId,
            Type = VoteType.UpVote,
            CommentId = TestCommentId,
            AuthorId = TestOtherVotedUserId
        };

        private readonly Comment testComment = new Comment()
        {
            Id = TestCommentId,
            Content = TestCommentContent,
            AuthorId = TestCommentAuthorId,
            Author = new ApplicationUser()
            {
                Id = TestCommentAuthorId,
                UserName = "TestCommentAuthor",
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
                Id = TestCategoryPostId,
                Title = "Test Post Title",
                Type = PostType.Text,
                ImageUrl = "TestPostImageUrl",
                Content = "Test Post Content",
                AuthorId = TestPostAuthorId,
                CategoryId = TestCategoryPostId
            }
        };

        private readonly ApplicationUser testVotedUser = new ApplicationUser()
        {
            Id = TestVotedUserId,
            UserName = "TestVotedUser",
            Email = "testvoteduser@mail.com",
            FirstName = "Test",
            SecondName = "Voted",
            LastName = "User",
            BirthDate = new DateTime(1999, 1, 1),
            Age = 23,
            Gender = GenderType.Male,
            Biography = "Test Author Biography",
            ProfilePictureURL = "ProfilePictureURL",
            EmailConfirmed = true
        };

        private readonly ApplicationUser testOtherVotedUser = new ApplicationUser()
        {
            Id = TestOtherVotedUserId,
            UserName = "TestOtherVotedUser",
            Email = "testvoteduser@mail.com",
            FirstName = "Test",
            SecondName = "OtherVoted",
            LastName = "User",
            BirthDate = new DateTime(1999, 1, 1),
            Age = 23,
            Gender = GenderType.Female,
            Biography = "Test Author Biography",
            ProfilePictureURL = "ProfilePictureURL",
            EmailConfirmed = true
        };

        [Fact]
        public async Task VoteAsync_ShouldAddUpVote()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            
            using IDeletableEntityRepository<CommentVote> commentVotesRepository = new EfDeletableEntityRepository<CommentVote>(dbContext);
            CommentVotesService service = new CommentVotesService(commentVotesRepository);

            await service.VoteAsync(TestCommentId, TestVotedUserId, true);
            CommentVote? commentVote = await dbContext.CommentVotes.FirstOrDefaultAsync(v => v.AuthorId == TestVotedUserId && v.CommentId = TestCommentId);

            Assert.NotNull(commentVote);
            Assert.Equal(TestVoteId, commentVote.Id);
            Assert.Equal(TestVotedUserId, commentVote.AuthorId);
            Assert.Equal(TestCommentId, commentVote.CommentId);
            Assert.Equal(VoteType.UpVote, commentVote.Type);
        }

        [Fact]
        public async Task VoteAsync_ShouldAddDownVote()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<CommentVote> commentVotesRepository = new EfDeletableEntityRepository<CommentVote>(dbContext);
            CommentVotesService service = new CommentVotesService(commentVotesRepository);

            await service.VoteAsync(TestCommentId, TestVotedUserId, false);
            CommentVote? commentVote = await dbContext.CommentVotes.FirstOrDefaultAsync(v => v.AuthorId == TestVotedUserId && v.CommentId = TestCommentId);

            Assert.NotNull(commentVote);
            Assert.Equal(TestVoteId, commentVote.Id);
            Assert.Equal(TestVotedUserId, commentVote.AuthorId);
            Assert.Equal(TestCommentId, commentVote.CommentId);
            Assert.Equal(VoteType.DownVote, commentVote.Type);
        }

        [Fact]
        public async Task VoteAsync_ShouldChangeVote()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.CommentVotes.AddAsync(this.testCommentVote);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentVote> commentVotesRepository = new EfDeletableEntityRepository<CommentVote>(dbContext);
            CommentVotesService service = new CommentVotesService(commentVotesRepository);

            await service.VoteAsync(TestCommentId, TestVotedUserId, false);
            CommentVote? commentVote = await dbContext.CommentVotes.FirstOrDefaultAsync(v => v.AuthorId == TestVotedUserId && v.CommentId = TestCommentId);

            Assert.NotNull(commentVote);
            Assert.Equal(TestVoteId, commentVote.Id);
            Assert.Equal(TestVotedUserId, commentVote.AuthorId);
            Assert.Equal(TestCommentId, commentVote.CommentId);
            Assert.Equal(VoteType.DownVote, commentVote.Type);
        }

        [Fact]
        public async Task GetVotes_ShouldGetVotesSum()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.CommentVotes.AddAsync(this.testCommentVote);
            await dbContext.CommentVotes.AddAsync(this.testOtherCommentVote);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentVote> commentVotesRepository = new EfDeletableEntityRepository<CommentVote>(dbContext);
            CommentVotesService service = new CommentVotesService(commentVotesRepository);

            int votesSum = service.GetVotes(TestCommentId);
            
            Assert.Equal(2, votesSum);
            Assert.Equal(2, dbContext.CommentVotes.Count());
        }

        [Fact]
        public async Task VoteAsync_ShouldNotCountVote_IfVoted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.CommentVotes.AddAsync(this.testCommentVote);
            await dbContext.CommentVotes.AddAsync(this.testOtherCommentVote);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentVote> commentVotesRepository = new EfDeletableEntityRepository<CommentVote>(dbContext);
            CommentVotesService service = new CommentVotesService(commentVotesRepository);

            await service.VoteAsync(TestCommentId, TestVotedUserId, true);
            CommentVote? commentVote = await dbContext.CommentVotes.FirstOrDefaultAsync(v => v.AuthorId == TestVotedUserId && v.CommentId = TestCommentId);
            int votesSum = service.GetVotes(TestCommentId);

            Assert.NotNull(commentVote);
            Assert.Equal(TestVoteId, commentVote.Id);
            Assert.Equal(TestVotedUserId, commentVote.AuthorId);
            Assert.Equal(TestCommentId, commentVote.CommentId);
            Assert.Equal(VoteType.UpVote, commentVote.Type);
            Assert.Equal(2, votesSum);
            Assert.Equal(2, dbContext.CommentVotes.Count());
        }

        [Fact]
        public async Task VoteAsync_ShouldCountVote_IfChangeVote()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.CommentVotes.AddAsync(this.testCommentVote);
            await dbContext.CommentVotes.AddAsync(this.testOtherCommentVote);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<CommentVote> commentVotesRepository = new EfDeletableEntityRepository<CommentVote>(dbContext);
            CommentVotesService service = new CommentVotesService(commentVotesRepository);

            await service.VoteAsync(TestCommentId, TestVotedUserId, false);
            CommentVote? commentVote = await dbContext.CommentVotes.FirstOrDefaultAsync(v => v.AuthorId == TestVotedUserId && v.CommentId = TestCommentId);
            int votesSum = service.GetVotes(TestCommentId);

            Assert.NotNull(commentVote);
            Assert.Equal(TestVoteId, commentVote.Id);
            Assert.Equal(TestVotedUserId, commentVote.AuthorId);
            Assert.Equal(TestCommentId, commentVote.CommentId);
            Assert.Equal(VoteType.DownVote, commentVote.Type);
            Assert.Equal(0, votesSum);
            Assert.Equal(2, dbContext.CommentVotes.Count());
        }
    }
}