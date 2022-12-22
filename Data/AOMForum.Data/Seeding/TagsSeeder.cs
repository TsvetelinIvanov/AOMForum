using AOMForum.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AOMForum.Data.Seeding
{
    public class TagsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Tags.AnyAsync())
            {
                return;
            }

            List<Tag> tags = new List<Tag>
            {
                new Tag { Name = "Автоматизация" },
                new Tag { Name = "Оптимизация" },
                new Tag { Name = "Меритокрация" },
                new Tag { Name = "Демокрация" },
                new Tag { Name = "Либерализъм" },
                new Tag { Name = "Неолиберализъм" },
                new Tag { Name = "Консерватизъм" },
                new Tag { Name = "Неоконсерватизъм" },
                new Tag { Name = "Геополитика" },
                new Tag { Name = "Глобализъм" },
                new Tag { Name = "Глобализация" },
                new Tag { Name = "Екология" },                
                new Tag { Name = "Климат" },
                new Tag { Name = "Природа" },
                new Tag { Name = "Ресурси" },
                new Tag { Name = "Храна" },
                new Tag { Name = "Горива" },
                new Tag { Name = "Логистика" },
                new Tag { Name = "Транспорт" },
                new Tag { Name = "Замърсяване" },
                new Tag { Name = "Затопляне" },
                new Tag { Name = "Пари" },
                new Tag { Name = "Валута" },
                new Tag { Name = "Евро" },
                new Tag { Name = "Лев" },
                new Tag { Name = "Икономика" },
                new Tag { Name = "Демография" },
                new Tag { Name = "Смъртност" },
                new Tag { Name = "Раждаемост" },
                new Tag { Name = "Емиграция" },
                new Tag { Name = "Семейство" },
            };

            await dbContext.AddRangeAsync(tags);
        }
    }
}