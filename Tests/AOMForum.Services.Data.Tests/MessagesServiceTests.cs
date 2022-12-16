using AOMForum.Data.Common.Repositories;
using AOMForum.Data;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using AOMForum.Data.Repositories;
using AOMForum.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using AOMForum.Web.Models.Messages;

namespace AOMForum.Services.Data.Tests
{
    public class MessagesServiceTests
    {
        private const int TestMessageId = 10;
        private const int TestOtherMessageId = 11;
        private const string TestUserId = "TestUserId";
        private const string TestOtherUserId = "TestOtherUserId";
        private const string TestPresentUserId = "TestOtherUserId";
        private const string TestMessageContent = "Test Message Content";
        private const string TestOtherMessageContent = "Test Other Message Content";
        private const string TestPresentMessageContent = "Test Present Message Content";

        private readonly ApplicationUser testUser = new ApplicationUser()
        {
            Id = TestUserId,
            UserName = "TestUser",
            Email = "testuser@mail.com",
            FirstName = "Test",
            SecondName = "Message",
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
            Email = "testuser@mail.com",
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
            Gender = GenderType.Female,
            Biography = "Test User Biography",
            ProfilePictureURL = "ProfilePictureURL",
            EmailConfirmed = true
        };

        Message testMessage = new Message()
        {
            Id = TestMessageId,
            Content = TestMessageContent,
            SenderId = TestUserId,
            ReceiverId = TestOtherUserId
        };

        Message testOtherMessage = new Message()
        {
            Id = TestOtherMessageId,
            Content = TestOtherMessageContent,
            SenderId = TestOtherUserId,
            ReceiverId = TestUserId
        };

        Message testPresentMessage = new Message()
        {
            Id = TestOtherMessageId,
            Content = TestPresentMessageContent,
            SenderId = TestPresentUserId,
            ReceiverId = TestUserId
        };

        [Fact]
        public async Task GetMessageInputModelAsync_ShouldReturnExpectedMessageInputModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();
            
            using IDeletableEntityRepository<Message> messagesRepository = new EfDeletableEntityRepository<Message>(dbContext);
            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            MessagesService service = new MessagesService(messagesRepository, usersRepository);

            MessageInputModel actualModel = await service.GetMessageInputModelAsync(TestUserId);

            Assert.NotNull(actualModel);
            Assert.Equal(2, actualModel.MessagePartners.Count());
            Assert.Contains(actualModel.MessagePartners, mp => mp.Id == TestOtherUserId);
            Assert.Contains(actualModel.MessagePartners, mp => mp.Id == TestPresentUserId);
        }

        [Fact]
        public async Task GetMessageInputModelAsync_ShouldReturnExpectedMessageInputModel_WithEmpyPartners()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);            
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Message> messagesRepository = new EfDeletableEntityRepository<Message>(dbContext);
            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            MessagesService service = new MessagesService(messagesRepository, usersRepository);

            MessageInputModel actualModel = await service.GetMessageInputModelAsync(TestUserId);

            Assert.NotNull(actualModel);
            Assert.Empty(actualModel.MessagePartners);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateMessage()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<Message> messagesRepository = new EfDeletableEntityRepository<Message>(dbContext);
            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            MessagesService service = new MessagesService(messagesRepository, usersRepository);

            bool isCreated = await service.CreateAsync(TestMessageContent, TestUserId, TestOtherUserId);
            Message? message = await dbContext.Messages.FirstOrDefaultAsync(m => m.Content == TestMessageContent && m.SenderId == TestUserId && m.ReceiverId == TestOtherUserId);

            Assert.NotNull(message);            
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnTrue_IfCreateMessage()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);

            using IDeletableEntityRepository<Message> messagesRepository = new EfDeletableEntityRepository<Message>(dbContext);
            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            MessagesService service = new MessagesService(messagesRepository, usersRepository);

            bool isCreated = await service.CreateAsync(TestMessageContent, TestUserId, TestOtherUserId);
            Message? message = await dbContext.Messages.FirstOrDefaultAsync(m => m.Content == TestMessageContent && m.SenderId == TestUserId && m.ReceiverId == TestOtherUserId);

            Assert.True(isCreated);
            Assert.NotNull(message);
        }

        [Fact]
        public async Task GetPartnerAllMessagesModelAsync_ShouldReturnExpectedPartnerAllMessagesModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            await dbContext.Messages.AddAsync(this.testMessage);
            await dbContext.Messages.AddAsync(this.testOtherMessage);
            await dbContext.Messages.AddAsync(this.testPresentMessage);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Message> messagesRepository = new EfDeletableEntityRepository<Message>(dbContext);
            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            MessagesService service = new MessagesService(messagesRepository, usersRepository);

            PartnerAllMessagesModel? actualModel = await service.GetPartnerAllMessagesModelAsync(TestUserId, TestOtherUserId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestOtherUserId, actualModel.MessagePartner.Id);
            Assert.Equal(2, actualModel.Messages.Count());
            Assert.Contains(actualModel.Messages, m => m.Content == TestMessageContent);
            Assert.Contains(actualModel.Messages, m => m.Content == TestOtherMessageContent);
        }

        [Fact]
        public async Task GetPartnerAllMessagesModelAsync_ShouldReturnNull_IfInexitantPartner()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);            
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();
            
            await dbContext.Messages.AddAsync(this.testPresentMessage);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Message> messagesRepository = new EfDeletableEntityRepository<Message>(dbContext);
            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            MessagesService service = new MessagesService(messagesRepository, usersRepository);

            PartnerAllMessagesModel? actualModel = await service.GetPartnerAllMessagesModelAsync(TestUserId, TestOtherUserId);

            Assert.Null(actualModel);
        }

        [Fact]
        public async Task GetLastMessageModelsAsync_ShouldReturnExpectedLastMessageModels()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            await dbContext.Messages.AddAsync(this.testMessage);
            await dbContext.Messages.AddAsync(this.testOtherMessage);
            await dbContext.Messages.AddAsync(this.testPresentMessage);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Message> messagesRepository = new EfDeletableEntityRepository<Message>(dbContext);
            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            MessagesService service = new MessagesService(messagesRepository, usersRepository);

            IEnumerable<LastMessageModel?> actualModels = await service.GetLastMessageModelsAsync(TestUserId);

            Assert.NotNull(actualModels);            
            Assert.Equal(2, actualModels.Count());
            Assert.Contains(actualModels, lm => lm.Id == TestOtherUserId);
            Assert.Contains(actualModels, lm => lm.Id == TestPresentUserId);
            Assert.Contains(actualModels, m => m.LastMessage == TestOtherMessageContent);
            Assert.Contains(actualModels, m => m.LastMessage == TestPresentMessageContent);
        }

        [Fact]
        public async Task GetMessagePartnerModelAsync_ShouldReturnExpectedMessagePartnerModel()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);
            await dbContext.Users.AddAsync(this.testOtherUser);
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Message> messagesRepository = new EfDeletableEntityRepository<Message>(dbContext);
            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            MessagesService service = new MessagesService(messagesRepository, usersRepository);

            MessagePartnerModel? actualModel = await service.GetMessagePartnerModelAsync(TestUserId);

            Assert.NotNull(actualModel);
            Assert.Equal(TestUserId, actualModel.Id);
            Assert.Equal(this.testUser.UserName, actualModel.UserName);
            Assert.Equal(this.testUser.ProfilePictureURL, actualModel.ProfilePictureURL);
        }

        [Fact]
        public async Task GetMessagePartnerModelAsync_ShouldReturnNull_IfInexistant()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            await dbContext.Users.AddAsync(this.testUser);            
            await dbContext.Users.AddAsync(this.testPresentUser);
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Message> messagesRepository = new EfDeletableEntityRepository<Message>(dbContext);
            using IDeletableEntityRepository<ApplicationUser> usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            MessagesService service = new MessagesService(messagesRepository, usersRepository);

            MessagePartnerModel? actualModel = await service.GetMessagePartnerModelAsync(TestOtherUserId);

            Assert.Null(actualModel);
        }
    }
}