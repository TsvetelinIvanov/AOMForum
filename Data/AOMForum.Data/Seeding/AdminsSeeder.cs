using AOMForum.Common;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AOMForum.Data.Seeding
{
    public class AdminsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser>? userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            if (userManager == null)
            {
                throw new NullReferenceException(nameof(userManager));
            }

            RoleManager<ApplicationRole>? roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
            if (userManager == null)
            {
                throw new NullReferenceException(nameof(roleManager));
            }

            bool isExistingAdmin = await userManager.Users.AnyAsync(u => u.UserName == "Админ");
            if (!isExistingAdmin)
            {
                ApplicationUser admin = new ApplicationUser
                {
                    UserName = "Админ",
                    Email = "admin@mail.com",
                    FirstName = "Господин",
                    SecondName = "Администраторов",
                    LastName = "Админов",
                    BirthDate = new DateTime(1981, 4, 12),
                    Age = 41,
                    Gender = GenderType.Male,
                    Biography = "Роден съм в град Берковица, където завърших основното си образование и продължих обучението си в в Монтана, след което се насочих към IT сферата.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(admin, "admin1");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }

                bool isRoleExists = await roleManager.RoleExistsAsync(GlobalConstants.AdministratorRoleName);
                if (isRoleExists)
                {
                    await userManager.AddToRoleAsync(admin, GlobalConstants.AdministratorRoleName);
                }
            }

            bool isExistingYoan = await userManager.Users.AnyAsync(u => u.UserName == "Йоан");
            if (!isExistingAdmin)
            {
                ApplicationUser admin = new ApplicationUser
                {
                    UserName = "Йоан",
                    Email = "ivanivanov@aom.bg",
                    FirstName = "Иван",
                    SecondName = "Иванов",
                    LastName = "Иванов",
                    BirthDate = new DateTime(1980, 4, 7),
                    Age = 42,
                    Gender = GenderType.Male,
                    Biography = "Роден съм в град Монтана. Имам висше образование от Историческия (бакалавър и магистър) и Педагогическия (магистър) факултети на Софийски университет \"Св. Климент Охридски\". Работя като учител по \"История и цивилизация\" в Софийска гимназия по хлебни и сладкарски технологии. В момента съм Главен координатор и Координатор по икономически политики на партия АОМ.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(admin, "admin1");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }

                bool isRoleExists = await roleManager.RoleExistsAsync(GlobalConstants.AdministratorRoleName);
                if (isRoleExists)
                {
                    await userManager.AddToRoleAsync(admin, GlobalConstants.AdministratorRoleName);
                }
            }
        }
    }
}