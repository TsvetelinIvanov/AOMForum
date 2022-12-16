using AOMForum.Data.Models.Enums;
using AOMForum.Data.Models;
using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Repositories;
using AOMForum.Data;
using AOMForum.Services.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace AOMForum.Services.Data.Tests
{
    public class PostVotesServiceTests
    {
        private const int TestVoteId = 10;
        private const int TestOtherVoteId = 11;
        private const int TestPostId = 100;        
        private const int TestCategoryId = 1000;
        private const string TestVotedUserId = "TestVotedUserId";
        private const string TestOtherVotedUserId = "TestOtherVotedUserId";
        private const string TestPostAuthorId = "TestPostAuthorId";        
        private const string TestPostTitle = "Test Post Title";
        private const string TestPostContent = "Test Post Content";

        private readonly PostVote testPostVote = new PostVote()
        {
            Id = TestVoteId,
            Type = VoteType.UpVote,
            PostId = TestPostId,
            AuthorId = TestVotedUserId
        };

        private readonly PostVote testOtherPostVote = new PostVote()
        {
            Id = TestOtherVoteId,
            Type = VoteType.UpVote,
            PostId = TestPostId,
            AuthorId = TestOtherVotedUserId
        };

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
            CategoryId = TestCategoryId,
            Category = new Category()
            {
                Id = TestCategoryId,
                Name = "Test Category Name",
                Description = "Test Category Description",
                ImageUrl = "TestCategoryImageUrl"
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

            using IDeletableEntityRepository<PostVote> postVotesRepository = new EfDeletableEntityRepository<PostVote>(dbContext);
            PostVotesService service = new PostVotesService(postVotesRepository);

            await service.VoteAsync(TestPostId, TestVotedUserId, true);
            PostVote? postVote = await dbContext.PostVotes.FirstOrDefaultAsync(v => v.AuthorId == TestVotedUserId && v.PostId == TestPostId);

            Assert.NotNull(postVote);
            Assert.Equal(TestVoteId, postVote.Id);
            Assert.Equal(TestVotedUserId, postVote.AuthorId);
            Assert.Equal(TestPostId, postVote.PostId);
            Assert.Equal(VoteType.UpVote, postVote.Type);
        }

        [Fact]
        public async Task VoteAsync_ShouldAddDownVote()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<PostVote> postVotesRepository = new EfDeletableEntityRepository<PostVote>(dbContext);
            PostVotesService service = new PostVotesService(postVotesRepository);

            await service.VoteAsync(TestPostId, TestVotedUserId, false);
            PostVote? postVote = await dbContext.PostVotes.FirstOrDefaultAsync(v => v.AuthorId == TestVotedUserId && v.PostId == TestPostId);

            Assert.NotNull(postVote);
            Assert.Equal(TestVoteId, postVote.Id);
            Assert.Equal(TestVotedUserId, postVote.AuthorId);
            Assert.Equal(TestPostId, postVote.PostId);
            Assert.Equal(VoteType.DownVote, postVote.Type);
        }

        [Fact]
        public async Task VoteAsync_ShouldChangeVote()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.PostVotes.AddAsync(this.testPostVote);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostVote> postVotesRepository = new EfDeletableEntityRepository<PostVote>(dbContext);
            PostVotesService service = new PostVotesService(postVotesRepository);

            await service.VoteAsync(TestPostId, TestVotedUserId, false);
            PostVote? postVote = await dbContext.PostVotes.FirstOrDefaultAsync(v => v.AuthorId == TestVotedUserId && v.PostId == TestPostId);

            Assert.NotNull(postVote);
            Assert.Equal(TestVoteId, postVote.Id);
            Assert.Equal(TestVotedUserId, postVote.AuthorId);
            Assert.Equal(TestPostId, postVote.PostId);
            Assert.Equal(VoteType.DownVote, postVote.Type);
        }

        [Fact]
        public async Task GetVotes_ShouldGetVotesSum()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.PostVotes.AddAsync(this.testPostVote);
            await dbContext.PostVotes.AddAsync(this.testOtherPostVote);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostVote> postVotesRepository = new EfDeletableEntityRepository<PostVote>(dbContext);
            PostVotesService service = new PostVotesService(postVotesRepository);

            int votesSum = service.GetVotes(TestPostId);

            Assert.Equal(2, votesSum);
            Assert.Equal(2, dbContext.PostVotes.Count());
        }

        [Fact]
        public async Task VoteAsync_ShouldNotCountVote_IfVoted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.PostVotes.AddAsync(this.testPostVote);
            await dbContext.PostVotes.AddAsync(this.testOtherPostVote);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostVote> postVotesRepository = new EfDeletableEntityRepository<PostVote>(dbContext);
            PostVotesService service = new PostVotesService(postVotesRepository);

            await service.VoteAsync(TestPostId, TestVotedUserId, true);
            PostVote? postVote = await dbContext.PostVotes.FirstOrDefaultAsync(v => v.AuthorId == TestVotedUserId && v.PostId == TestPostId);
            int votesSum = service.GetVotes(TestPostId);

            Assert.NotNull(postVote);
            Assert.Equal(TestVoteId, postVote.Id);
            Assert.Equal(TestVotedUserId, postVote.AuthorId);
            Assert.Equal(TestPostId, postVote.PostId);
            Assert.Equal(VoteType.UpVote, postVote.Type);
            Assert.Equal(2, votesSum);
            Assert.Equal(2, dbContext.PostVotes.Count());
        }

        [Fact]
        public async Task VoteAsync_ShouldCountVote_IfChangeVote()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.PostVotes.AddAsync(this.testPostVote);
            await dbContext.PostVotes.AddAsync(this.testOtherPostVote);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<PostVote> postVotesRepository = new EfDeletableEntityRepository<PostVote>(dbContext);
            PostVotesService service = new PostVotesService(postVotesRepository);

            await service.VoteAsync(TestPostId, TestVotedUserId, false);
            PostVote? postVote = await dbContext.PostVotes.FirstOrDefaultAsync(v => v.AuthorId == TestVotedUserId && v.PostId == TestPostId);
            int votesSum = service.GetVotes(TestPostId);

            Assert.NotNull(postVote);
            Assert.Equal(TestVoteId, postVote.Id);
            Assert.Equal(TestVotedUserId, postVote.AuthorId);
            Assert.Equal(TestPostId, postVote.PostId);
            Assert.Equal(VoteType.DownVote, postVote.Type);
            Assert.Equal(0, votesSum);
            Assert.Equal(2, dbContext.PostVotes.Count());
        }
    }
}