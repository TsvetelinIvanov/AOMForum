using AOMForum.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AOMForum.Data.Seeding
{
    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Categories.AnyAsync())
            {
                return;
            }

            List<Category> categories = new List<Category>
            {
                new Category
                {
                    Name = "Оптимизиране на бюрокрацията",
                    Description = "Прекомерното увеличаване на бюрократичния апарат в България и Европейския съюз се отразява негативно на икономическото им развитие - администрацията трябва да бъде намалена, а законовата рамка опростена.",
                    ImageUrl = "~/images/bulgaria-flag.png"
                },
                new Category
                { 
                    Name = "Фискална политика",
                    Description = "Системата по която работят българските и европейскитебюджетни политики не се отразяват добре на икономическото развитие - държавните дългове се увеличават, а икономиката стагнира - възможен изход от кризата е коренна промяна във фискалната политика - намаляване на данъчната тежест и държавната намеса.",
                    ImageUrl = "~/images/flag-bulgaria.jpg"
                },
                new Category
                {
                    Name = "Еврозона",
                    Description = "Предимства и недостатъци на Еврозоната и плюсове и минуси за България при влизането и в нея.",
                    ImageUrl = "~/images/mediteranium.jpg"
                },
                new Category
                {
                    Name = "Климатични промени",
                    Description = "Климатът очевидно се променя, но дали това има нещо общо с човешката дейност и дали скъпите мерки които се вземат ще му повлиаят или само ще отклонят огромни финансови ресурси към определени субекти - сигурно е само едно - ще се отразят негативно върху икономическото развитие.",
                    ImageUrl = "~/images/flower.jpg"
                },                
                new Category
                {
                    Name = "Демографска криза",
                    Description = "Населението на България драстично намалява и трябва да бъдат взети спешни мерки това да бъде променено - това е една от малкото области където е нужна държавна намеса.",
                    ImageUrl = "~/images/bulgaria-map.png"
                }
            };

            await dbContext.AddRangeAsync(categories);
        }
    }
}