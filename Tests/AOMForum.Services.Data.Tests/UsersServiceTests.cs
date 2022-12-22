using AOMForum.Data.Common.Repositories;
using AOMForum.Data;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using AOMForum.Data.Repositories;
using AOMForum.Services.Data.Services;
using AOMForum.Web.Models.Categories;
using Microsoft.EntityFrameworkCore;
using AOMForum.Web.Models.UserRelationships;
using AOMForum.Web.Models.Home;

namespace AOMForum.Services.Data.Tests
{
    public class UsersServiceTests
    {        
        private const string TestUserId = "TestUserId";
        private const string TestOtherUserId = "TestOtherUserId";
        private const string TestPresentUserId = "TestPresentUserId";
        private const int TestRelationshipId = 10000;
        private const int TestPostId = 10;
        private const int TestOtherPostId = 11;
        private const int TestCommentId = 100;
        private const int TestOtherCommentId = 101;
        private const int TestCategoryId = 1000;

        private readonly ApplicationUser testUser = new ApplicationUser()
        {
            Id = TestUserId,
            UserName = "TestUser",
            Email = "testuser@mail.com",
            FirstName = "Test",
            SecondName = "User",
            LastName = "User",
            BirthDate = new DateTime(1999, 1, 1),
            Age = 23,
            Gender = GenderType.Male,
            Biography = "Test User Biography",
            ProfilePictureURL = "ProfilePictureURL",
            EmailConfirmed = true
        };

        private readonly ApplicationUser testOtherUser = new ApplicationUser()
        {
            Id = TestOtherUserId,
            UserName = "TestOtherUser",
            Email = "test@mail.com",
            FirstName = "Test",
            SecondName = "Other",
            LastName = "User",
            BirthDate = new DateTime(1999, 1, 1),
            Age = 23,
            Gender = GenderType.Female,
            Biography = "Test User Biography",
            ProfilePictureURL = "ProfilePictureURL",
            EmailConfirmed = true
        };

        private readonly ApplicationUser testPresentUser = new ApplicationUser()
        {
            Id = TestPresentUserId,
            UserName = "TestPresentUser",
            Email = "testuser@mail.com",
            FirstName = "Test",
            SecondName = "Present",
            LastName = "User",
            BirthDate = new DateTime(1999, 1, 1),
            Age = 23,
            Gender = GenderType.Male,
            Biography = "Test User Biography",
            ProfilePictureURL = "ProfilePictureURL",
            EmailConfirmed = true
        };

        private readonly Relationship testRelationship = new Relationship()
        {
            Id = TestRelationshipId,
            LeaderId = TestUserId,
            FollowerId = TestOtherUserId
        };

        private readonly Post testPost = new Post()
        {
            Id = TestPostId,
            Title = "Test Post Title",
            Type = PostType.Text,
            ImageUrl = "TestPostImageUrl",
            Content = "Test Post Content",
            AuthorId = TestUserId,            
            CategoryId = TestCategoryId
        };

        private readonly Post testOtherPost = new Post()
        {
            Id = TestOtherPostId,
            Title = "Test Other Post Title",
            Type = PostType.Text,
            ImageUrl = "TestPostImageUrl",
            Content = "Test Other Post Content",
            AuthorId = TestOtherUserId,
            CategoryId = TestCategoryId
        };

        private readonly Comment testComment = new Comment()
        {
            Id = TestCommentId,
            Content = "Test Comment Content",
            AuthorId = TestOtherUserId,            
            PostId = TestPostId
        };

        private readonly Comment testOtherComment = new Comment()
        {
            Id = TestOtherCommentId,
            Content = "Test Other Comment Content",
            AuthorId = TestOtherUserId,
            PostId = TestPostId
        };

        private readonly Category testCategory = new Category()
        {
            Id = TestCategoryId,
            Name = "Test Category Name",
            Description = "Test Category Description",
            ImageUrl = "TestCategoryImageUrl"           
        };

        [Fact]
        public async Task GetUserPostsIndexViewModelAsync_ShouldReturnExpectedUserPostsIndexViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserPostsIndexViewModel? actualModel = await service.GetUserPostsIndexViewModelAsync(TestUserId, TestOtherUserId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestUserId, actualModel.Id);
            Assert.Equal(this.testUser.UserName, actualModel.UserName);
            Assert.Equal(this.testUser.ProfilePictureURL, actualModel.ProfilePictureURL);
            Assert.Equal(1, actualModel.PostsCount);
            Assert.Equal(0, actualModel.CommentsCount);
            Assert.Equal(0, actualModel.VotesCount);
            Assert.True(actualModel.IsFollowed);
            Assert.Equal(1, actualModel.FollowersCount);
            Assert.Equal(0, actualModel.FollowingsCount);            
        }

        [Fact]
        public async Task GetUserPostsIndexViewModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserPostsIndexViewModel? actualModel = await service.GetUserPostsIndexViewModelAsync(TestPresentUserId, TestOtherUserId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task GetUserCommentsIndexViewModelAsync_ShouldReturnExpectedUserCommentsIndexViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserCommentsIndexViewModel? actualModel = await service.GetUserCommentsIndexViewModelAsync(TestOtherUserId, TestUserId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestOtherUserId, actualModel.Id);
            Assert.Equal(this.testOtherUser.UserName, actualModel.UserName);
            Assert.Equal(this.testOtherUser.ProfilePictureURL, actualModel.ProfilePictureURL);
            Assert.Equal(1, actualModel.PostsCount);
            Assert.Equal(2, actualModel.CommentsCount);
            Assert.Equal(0, actualModel.VotesCount);
            Assert.False(actualModel.IsFollowed);
            Assert.Equal(0, actualModel.FollowersCount);
            Assert.Equal(1, actualModel.FollowingsCount);
        }

        [Fact]
        public async Task GetUserCommentsIndexViewModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserCommentsIndexViewModel? actualModel = await service.GetUserCommentsIndexViewModelAsync(TestPresentUserId, TestUserId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task GetUserFollowersIndexViewModelAsync_ShouldReturnExpectedUserFollowersIndexViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserFollowersIndexViewModel? actualModel = await service.GetUserFollowersIndexViewModelAsync(TestUserId, TestOtherUserId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestUserId, actualModel.Id);
            Assert.Equal(this.testUser.UserName, actualModel.UserName);
            Assert.Equal(this.testUser.ProfilePictureURL, actualModel.ProfilePictureURL);
            Assert.Equal(1, actualModel.PostsCount);
            Assert.Equal(0, actualModel.CommentsCount);
            Assert.Equal(0, actualModel.VotesCount);
            Assert.True(actualModel.IsFollowed);
            Assert.Equal(1, actualModel.FollowersCount);
            Assert.Equal(0, actualModel.FollowingsCount);
            Assert.Single(actualModel.Followers);
            Assert.Contains(actualModel.Followers, u => u.Id == TestOtherUserId);
        }

        [Fact]
        public async Task GetUserFollowersIndexViewModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserFollowersIndexViewModel? actualModel = await service.GetUserFollowersIndexViewModelAsync(TestPresentUserId, TestOtherUserId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task GetUserFollowingsIndexViewModelAsync_ShouldReturnExpectedUserFollowingsIndexViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserFollowingsIndexViewModel? actualModel = await service.GetUserFollowingsIndexViewModelAsync(TestOtherUserId, TestUserId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestOtherUserId, actualModel.Id);
            Assert.Equal(this.testOtherUser.UserName, actualModel.UserName);
            Assert.Equal(this.testOtherUser.ProfilePictureURL, actualModel.ProfilePictureURL);
            Assert.Equal(1, actualModel.PostsCount);
            Assert.Equal(2, actualModel.CommentsCount);
            Assert.Equal(0, actualModel.VotesCount);
            Assert.False(actualModel.IsFollowed);
            Assert.Equal(0, actualModel.FollowersCount);
            Assert.Equal(1, actualModel.FollowingsCount);
            Assert.Single(actualModel.Followings);
            Assert.Contains(actualModel.Followings, u => u.Id == TestUserId);            
        }

        [Fact]
        public async Task GetUserFollowingsIndexViewModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserFollowingsIndexViewModel? actualModel = await service.GetUserFollowingsIndexViewModelAsync(TestPresentUserId, TestUserId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task GetUserDetailsViewModelAsync_ShouldReturnExpectedUserDetailsViewModelModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserDetailsViewModel? actualModel = await service.GetUserDetailsViewModelAsync(TestOtherUserId, TestUserId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestOtherUserId, actualModel.Id);
            Assert.Equal(this.testOtherUser.UserName, actualModel.UserName);
            Assert.Equal(this.testOtherUser.ProfilePictureURL, actualModel.ProfilePictureURL);
            Assert.Equal(1, actualModel.PostsCount);
            Assert.Equal(2, actualModel.CommentsCount);
            Assert.Equal(0, actualModel.VotesCount);
            Assert.False(actualModel.IsFollowed);
            Assert.Equal(0, actualModel.FollowersCount);
            Assert.Equal(1, actualModel.FollowingsCount);
            Assert.Empty(actualModel.Followers);
            Assert.Single(actualModel.Followings);
            Assert.Contains(actualModel.Followings, u => u.Id == TestUserId);
        }

        [Fact]
        public async Task GetUserDetailsViewModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserDetailsViewModel? actualModel = await service.GetUserDetailsViewModelAsync(TestPresentUserId, TestUserId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task FollowAsync_ShouldCreateRelationship()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isFollower = await service.FollowAsync(TestUserId, TestPresentUserId);
            Relationship? relationship = await dbContext.Relationships.Where(r => r.LeaderId == TestUserId && r.FollowerId == TestPresentUserId).FirstOrDefaultAsync();

            Assert.NotNull(relationship);
        }

        [Fact]
        public async Task FollowAsync_ShouldReturnTrue_IfCreateRelationship()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isFollower = await service.FollowAsync(TestUserId, TestPresentUserId);
            Relationship? relationship = await dbContext.Relationships.Where(r => r.LeaderId == TestUserId && r.FollowerId == TestPresentUserId).FirstOrDefaultAsync();

            Assert.True(isFollower);
            Assert.NotNull(relationship);
        }

        [Fact]
        public async Task FollowAsync_ShouldReturnFalse_IfInexistantLeader()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);            
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isFollower = await service.FollowAsync(TestUserId, TestPresentUserId);
            Relationship? relationship = await dbContext.Relationships.Where(r => r.LeaderId == TestUserId && r.FollowerId == TestPresentUserId).FirstOrDefaultAsync();

            Assert.False(isFollower);
            Assert.Null(relationship);
        }

        [Fact]
        public async Task FollowAsync_ShouldReturnFalse_IfRelationshipExistent()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isFollower = await service.FollowAsync(TestUserId, TestOtherUserId);

            Assert.False(isFollower);
        }

        [Fact]
        public async Task UnfollowAsync_ShouldDeleteRelationship()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isNoFollower = await service.UnfollowAsync(TestUserId, TestOtherUserId);
            Relationship? relationship = await dbContext.Relationships.Where(r => r.LeaderId == TestUserId && r.FollowerId == TestOtherUserId).FirstOrDefaultAsync();

            Assert.Null(relationship);
        }

        [Fact]
        public async Task UnfollowAsync_ShouldRetuenTrue_IfDeleteRelationship()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isNoFollower = await service.UnfollowAsync(TestUserId, TestOtherUserId);
            Relationship? relationship = await dbContext.Relationships.Where(r => r.LeaderId == TestUserId && r.FollowerId == TestOtherUserId).FirstOrDefaultAsync();

            Assert.True(isNoFollower);
            Assert.Null(relationship);
        }

        [Fact]
        public async Task UnfollowAsync_ShouldReturnFalse_IfInexistantLeader()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isNoFollower = await service.UnfollowAsync(TestUserId, TestOtherUserId);
            
            Assert.False(isNoFollower);            
        }

        [Fact]
        public async Task UnfollowAsync_ShouldReturnFalse_IfInexistantRelationship()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isNoFollower = await service.UnfollowAsync(TestUserId, TestPresentUserId);

            Assert.False(isNoFollower);
        }

        [Fact]
        public async Task GetUserListViewModelsAsync_ShouldReturnExpectedUserListViewModels()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            IEnumerable<UserListViewModel> actualModels = await service.GetUserListViewModelsAsync();

            Assert.NotNull(actualModels);
            Assert.Equal(3, actualModels.Count());
            Assert.Contains(actualModels, u => u.Id == TestUserId);
            Assert.Contains(actualModels, u => u.UserName == this.testUser.UserName);
            Assert.Contains(actualModels, u => u.FirstName == this.testUser.FirstName);
            Assert.Contains(actualModels, u => u.SecondName == this.testUser.SecondName);
            Assert.Contains(actualModels, u => u.LastName == this.testUser.LastName);
            Assert.Contains(actualModels, u => u.Biography == this.testUser.Biography);
            Assert.Contains(actualModels, u => u.ProfilePictureURL == this.testUser.ProfilePictureURL);            
            Assert.Contains(actualModels, u => u.Id == TestOtherUserId);
            Assert.Contains(actualModels, u => u.UserName == this.testOtherUser.UserName);
            Assert.Contains(actualModels, u => u.FirstName == this.testOtherUser.FirstName);
            Assert.Contains(actualModels, u => u.SecondName == this.testOtherUser.SecondName);
            Assert.Contains(actualModels, u => u.LastName == this.testOtherUser.LastName);
            Assert.Contains(actualModels, u => u.Id == TestPresentUserId);
            Assert.Contains(actualModels, u => u.UserName == this.testPresentUser.UserName);
            Assert.Contains(actualModels, u => u.FirstName == this.testPresentUser.FirstName);
            Assert.Contains(actualModels, u => u.SecondName == this.testPresentUser.SecondName);
            Assert.Contains(actualModels, u => u.LastName == this.testPresentUser.LastName);
        }

        [Fact]
        public async Task GetUserDeleteModelAsync_ShouldReturnExpectedUserDeleteModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserDeleteModel? actualModel = await service.GetUserDeleteModelAsync(TestUserId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestUserId, actualModel.Id);
            Assert.Equal(this.testUser.UserName, actualModel.UserName);
            Assert.Equal(this.testUser.Email, actualModel.Email);
            Assert.Equal(this.testUser.FirstName, actualModel.FirstName);
            Assert.Equal(this.testUser.SecondName, actualModel.SecondName);
            Assert.Equal(this.testUser.LastName, actualModel.LastName);
            Assert.Equal(this.testUser.ProfilePictureURL, actualModel.ProfilePictureURL);
        }

        [Fact]
        public async Task GetUserDeleteModelAsync_ShouldReturnReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserDeleteModel? actualModel = await service.GetUserDeleteModelAsync(TestUserId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isDeleted = await service.DeleteAsync(TestUserId);
            ApplicationUser? user = await dbContext.Users.FindAsync(TestUserId);

            Assert.NotNull(user);
            Assert.True(user.IsDeleted);
            Assert.Equal(this.testUser.UserName, user.UserName);
            Assert.Equal(this.testUser.FirstName, user.FirstName);
            Assert.Equal(this.testUser.SecondName, user.SecondName);
            Assert.Equal(this.testUser.LastName, user.LastName);
            Assert.Equal(this.testUser.ProfilePictureURL, user.ProfilePictureURL);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_IfDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isDeleted = await service.DeleteAsync(TestUserId);
            ApplicationUser? user = await dbContext.Users.FindAsync(TestUserId);

            Assert.True(isDeleted);
            Assert.NotNull(user);
            Assert.True(user.IsDeleted);
            Assert.Equal(this.testUser.UserName, user.UserName);
            Assert.Equal(this.testUser.FirstName, user.FirstName);
            Assert.Equal(this.testUser.SecondName, user.SecondName);
            Assert.Equal(this.testUser.LastName, user.LastName);
            Assert.Equal(this.testUser.ProfilePictureURL, user.ProfilePictureURL);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_IfNotDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isDeleted = await service.DeleteAsync(TestUserId);
            ApplicationUser? user = await dbContext.Users.FindAsync(TestUserId);

            Assert.False(isDeleted);
            Assert.Null(user);
            Assert.Equal(2, dbContext.Users.Count());
        }

        [Fact]
        public async Task GetUserListViewModelsForDeletedAsync_ShouldReturnExpectedListViewModelsForDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            this.testUser.IsDeleted = true;
            this.testOtherUser.IsDeleted = true;
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            IEnumerable<UserListViewModel> actualModels = await service.GetUserListViewModelsForDeletedAsync();

            Assert.NotNull(actualModels);
            Assert.Equal(2, actualModels.Count());
            Assert.Contains(actualModels, u => u.Id == TestUserId);
            Assert.Contains(actualModels, u => u.UserName == this.testUser.UserName);
            Assert.Contains(actualModels, u => u.FirstName == this.testUser.FirstName);
            Assert.Contains(actualModels, u => u.SecondName == this.testUser.SecondName);
            Assert.Contains(actualModels, u => u.LastName == this.testUser.LastName);
            Assert.Contains(actualModels, u => u.Biography == this.testUser.Biography);
            Assert.Contains(actualModels, u => u.ProfilePictureURL == this.testUser.ProfilePictureURL);
            Assert.Contains(actualModels, u => u.Id == TestOtherUserId);
            Assert.Contains(actualModels, u => u.UserName == this.testOtherUser.UserName);
            Assert.Contains(actualModels, u => u.FirstName == this.testOtherUser.FirstName);
            Assert.Contains(actualModels, u => u.SecondName == this.testOtherUser.SecondName);
            Assert.Contains(actualModels, u => u.LastName == this.testOtherUser.LastName);
        }

        [Fact]
        public async Task GetUserDeleteModelForDeletedAsync_ShouldReturnExpectedUserDeleteModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            this.testUser.IsDeleted = true;
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserDeleteModel? actualModel = await service.GetUserDeleteModelForDeletedAsync(TestUserId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestUserId, actualModel.Id);
            Assert.Equal(this.testUser.UserName, actualModel.UserName);
            Assert.Equal(this.testUser.Email, actualModel.Email);
            Assert.Equal(this.testUser.FirstName, actualModel.FirstName);
            Assert.Equal(this.testUser.SecondName, actualModel.SecondName);
            Assert.Equal(this.testUser.LastName, actualModel.LastName);
            Assert.Equal(this.testUser.ProfilePictureURL, actualModel.ProfilePictureURL);
        }

        [Fact]
        public async Task GetUserDeleteModelForDeletedAsync_ShouldReturnReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserDeleteModel? actualModel = await service.GetUserDeleteModelForDeletedAsync(TestUserId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task GetUserDeleteModelForDeletedAsync_ShouldReturnReturnNull_IfNotDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            UserDeleteModel? actualModel = await service.GetUserDeleteModelForDeletedAsync(TestUserId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task UndeleteAsync_ShouldRestore()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            this.testUser.IsDeleted = true;
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isRestored = await service.UndeleteAsync(TestUserId);
            ApplicationUser? user = await dbContext.Users.FindAsync(TestUserId);

            Assert.NotNull(user);
            Assert.False(user.IsDeleted);
            Assert.Equal(this.testUser.UserName, user.UserName);
            Assert.Equal(this.testUser.FirstName, user.FirstName);
            Assert.Equal(this.testUser.SecondName, user.SecondName);
            Assert.Equal(this.testUser.LastName, user.LastName);
            Assert.Equal(this.testUser.ProfilePictureURL, user.ProfilePictureURL);
        }

        [Fact]
        public async Task UndeleteAsync_ShouldReturnTrue_IfRestored()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            this.testUser.IsDeleted = true;
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isRestored = await service.UndeleteAsync(TestUserId);
            ApplicationUser? user = await dbContext.Users.FindAsync(TestUserId);

            Assert.True(isRestored);
            Assert.NotNull(user);
            Assert.False(user.IsDeleted);
            Assert.Equal(this.testUser.UserName, user.UserName);
            Assert.Equal(this.testUser.FirstName, user.FirstName);
            Assert.Equal(this.testUser.SecondName, user.SecondName);
            Assert.Equal(this.testUser.LastName, user.LastName);
            Assert.Equal(this.testUser.ProfilePictureURL, user.ProfilePictureURL);
        }

        [Fact]
        public async Task UndeleteAsync_ShouldReturnFalse_IfNotRestored()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            this.testUser.IsDeleted = true;
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isRestored = await service.UndeleteAsync(TestOtherUserId);
            ApplicationUser? otherUser = await dbContext.Users.FindAsync(TestOtherUserId);
            ApplicationUser? user = await dbContext.Users.FindAsync(TestUserId);

            Assert.False(isRestored);
            Assert.NotNull(otherUser);
            Assert.NotNull(user);
            Assert.False(otherUser.IsDeleted);
            Assert.True(user.IsDeleted);
        }

        [Fact]
        public async Task GetHomeViewModelAsync_ShouldReturnExpectedHomeViewModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Posts.AddAsync(this.testPost);
            await dbContext.Posts.AddAsync(this.testOtherPost);
            await dbContext.Comments.AddAsync(this.testComment);
            await dbContext.Comments.AddAsync(this.testOtherComment);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            HomeViewModel actualModel = await service.GetHomeViewModelAsync();

            Assert.NotNull(actualModel);
            Assert.Equal(2, actualModel.PostsCount);
            Assert.Equal(3, actualModel.UsersCount);
        }

        [Fact]
        public async Task IsUsernameUsedAsync_ShouldReturnFalse_IfNotUsed()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);            
            await dbContext.Users.AddAsync(this.testUser);            
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isUsed = await service.IsUsernameUsedAsync(this.testOtherUser.UserName);

            Assert.False(isUsed);
        }

        [Fact]
        public async Task IsUsernameUsedAsync_ShouldReturnTrue_IfUsed()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isUsed = await service.IsUsernameUsedAsync(this.testOtherUser.UserName);

            Assert.True(isUsed);
        }

        [Fact]
        public async Task IsDeletedAsync_ShouldReturnFalse_IfNotDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isDeleted = await service.IsDeletedAsync(this.testUser.UserName);

            Assert.False(isDeleted);
        }

        [Fact]
        public async Task IsDeletedAsync_ShouldReturnTrue_IfDeleted()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            this.testUser.IsDeleted = true;
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isDeleted = await service.IsDeletedAsync(this.testUser.UserName);

            Assert.True(isDeleted);
        }

        [Fact]
        public async Task GetTotalCountAsync_ShouldReturnUsersCount()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            int usersCount = await service.GetTotalCountAsync();

            Assert.Equal(3, usersCount);
        }

        [Fact]
        public async Task IsFollowedAlreadyAsync_ShouldReturnTrue_IfFollowed()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isFollowed = await service.IsFollowedAlreadyAsync(TestUserId, TestOtherUserId);

            Assert.True(isFollowed);
        }

        [Fact]
        public async Task IsFollowedAlreadyAsync_ShouldReturnFalse_IfNotFollowed()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            bool isFollowed = await service.IsFollowedAlreadyAsync(TestUserId, TestPresentUserId);

            Assert.False(isFollowed);
        }

        [Fact]
        public async Task GetFollowersCountAsync_ShouldReturnFollowersCount()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            int followersCount = await service.GetFollowersCountAsync(TestUserId);

            Assert.Equal(1, followersCount);
        }

        [Fact]
        public async Task GetFollowersCountAsync_ShouldReturnZero_IfNoFollowers()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            int followersCount = await service.GetFollowersCountAsync(TestPresentUserId);

            Assert.Equal(0, followersCount);
        }

        [Fact]
        public async Task GetFollowingsCountAsync_ShouldReturnFollowingsCount()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            int followingsCount = await service.GetFollowingsCountAsync(TestOtherUserId);

            Assert.Equal(1, followingsCount);
        }

        [Fact]
        public async Task GetFollowingsCountAsync_ShouldReturnZero_IfNoFollowings()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.Relationships.AddAsync(this.testRelationship);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            using IDeletableEntityRepository<Post> postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            using IDeletableEntityRepository<Comment> commentsRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            using IDeletableEntityRepository<Relationship> relationshipsRepository = new EfDeletableEntityRepository<Relationship>(dbContext);
            using IDeletableEntityRepository<ApplicationRole> rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(dbContext);
            UsersService service = new UsersService(usersRepository, postsRepository, commentsRepository, relationshipsRepository, rolesRepository);

            int followingsCount = await service.GetFollowingsCountAsync(TestPresentUserId);

            Assert.Equal(0, followingsCount);
        }
    }
}