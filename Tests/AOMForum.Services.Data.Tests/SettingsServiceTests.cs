using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models;
using AOMForum.Data.Repositories;
using AOMForum.Data;
using AOMForum.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AOMForum.Services.Data.Tests
{
    public class SettingsServiceTests
    {
        [Fact]
        public void GetCountShouldReturnCorrectNumber()
        {
            Mock<IDeletableEntityRepository<Setting>> mockRepository = new Mock<IDeletableEntityRepository<Setting>>();
            mockRepository.Setup(mr => mr.AllAsNoTracking()).Returns(new List<Setting>
                                                        {
                                                            new Setting(),
                                                            new Setting(),
                                                            new Setting(),
                                                        }.AsQueryable());
            SettingsService service = new SettingsService(mockRepository.Object);

            Assert.Equal(3, service.GetCount());
            mockRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectNumberUsingDbContext()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using ApplicationDbContext dbContext = new ApplicationDbContext(options);
            dbContext.Settings.Add(new Setting());
            dbContext.Settings.Add(new Setting());
            dbContext.Settings.Add(new Setting());
            await dbContext.SaveChangesAsync();

            using IDeletableEntityRepository<Setting> repository = new EfDeletableEntityRepository<Setting>(dbContext);
            SettingsService service = new SettingsService(repository);

            Assert.Equal(3, service.GetCount());
        }
    }
}