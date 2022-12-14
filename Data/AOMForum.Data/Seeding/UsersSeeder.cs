﻿using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AOMForum.Data.Seeding
{
    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser>? userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            if (userManager == null)
            {
                throw new NullReferenceException(nameof(userManager));
            }

            bool isExistingStoyan = await userManager.Users.AnyAsync(u => u.UserName == "Стоян1");
            if (!isExistingStoyan)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Стоян1",
                    Email = "stoyanduvarov@aom.bg",
                    FirstName = "Стоян",
                    SecondName = "Каменов",
                    LastName = "Дуваров",
                    BirthDate = new DateTime(1978, 11, 8),
                    Age = 44,
                    Gender = GenderType.Male,
                    Biography = "Роден съм в град София. Завършил съм специалност \"Гранична полиция\" в Академията на МВР-София. Сега съм началник смяна в Гранична полиция на летище София. В АОМ съм Координатор на политики по вътрешните работи.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            bool isExistingBoyko = await userManager.Users.AnyAsync(u => u.UserName == "Бойко1");
            if (!isExistingBoyko)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Бойко1",
                    Email = "boykoarmeyski@aom.bg",
                    FirstName = "Бойко",
                    SecondName = "Войников",
                    LastName = "Армейски",
                    BirthDate = new DateTime(1970, 5, 6),
                    Age = 52,
                    Gender = GenderType.Male,
                    Biography = "Роден съм в град Преслав. Завършил съм Национален военен университет \"Васил Левски\" във Велико Търново. В момента съм началник физическа охрана на група обекти във \"Форс Делта\" ООД и Координатор на политики по национална сигурност и отбрана в АОМ.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            bool isExistingYustinian = await userManager.Users.AnyAsync(u => u.UserName == "Юстиниан1");
            if (!isExistingYustinian)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Юстиниан1",
                    Email = "yustinianZakonov@aom.bg",
                    FirstName = "Юстиниан",
                    SecondName = "Управдов",
                    LastName = "Законов",
                    BirthDate = new DateTime(1985, 11, 14),
                    Age = 37,
                    Gender = GenderType.Male,
                    Biography = "Роден съм в град Плевен. Имам висше образование от Юридическия факултет (магистър) на Софийски университет \"Св. Климент Охридски\". Адвокатски сътрудник съм в адвокатска кантора \"Расташки\", а в АОМ съм Координатор на политики за правосъдие.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            bool isExistingSokrat = await userManager.Users.AnyAsync(u => u.UserName == "Сократ1");
            if (!isExistingSokrat)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Сократ1",
                    Email = "sokratplatonov@aom.bg",
                    FirstName = "Сократ",
                    SecondName = "Аристотелов",
                    LastName = "Платонов",
                    BirthDate = new DateTime(1991, 11, 1),
                    Age = 31,
                    Gender = GenderType.Male,
                    Biography = "Роден съм в град София. Имам висше образование от Философския факултет (бакалавър и магистър) на Софийски университет \"Св. Климент Охридски\". Сега съм учител по философски дисциплини в Софийска математическа гимназия \"Паисий Хилендарски\" и Координатор на политики за наука и култура в АОМ.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            bool isExistingArpa = await userManager.Users.AnyAsync(u => u.UserName == "Арпа1");
            if (!isExistingArpa)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Арпа1",
                    Email = "arpaefirova@aom.bg",
                    FirstName = "Арпа",
                    SecondName = "Меркуриева",
                    LastName = "Ефирова",
                    BirthDate = new DateTime(2000, 5, 7),
                    Age = 22,
                    Gender = GenderType.Female,
                    Biography = "Родена съм в град София. Завършила съм Националната гимназия за древни езици и култури \"Св. Константин-Кирил Философ\", а сега уча история в Софийски университет \"Св. Климент Охридски\" и съм Координатор на политики за медии и комуникации в АОМ.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            bool isExistingNikola = await userManager.Users.AnyAsync(u => u.UserName == "Никола1");
            if (!isExistingNikola)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Никола1",
                    Email = "nikoladaskalov@aom.bg",
                    FirstName = "Никола",
                    SecondName = "Априлов",
                    LastName = "Даскалов",
                    BirthDate = new DateTime(1960, 5, 24),
                    Age = 62,
                    Gender = GenderType.Male,
                    Biography = "Роден съм в град Русе. Имам висше образование от Педагогическия факултет (магистър) на Софийски университет \"Св. Климент Охридски\". Работя като начален учител в 119 СУ \"Акад. Михаил Арнаудов\". В АОМ съм Координатор на политики за образование.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            bool isExistingMaria = await userManager.Users.AnyAsync(u => u.UserName == "Мария1");
            if (!isExistingMaria)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Мария1",
                    Email = "mariagospojina@aom.bg",
                    FirstName = "Мария",
                    SecondName = "Яковова",
                    LastName = "Госпожина",
                    BirthDate = new DateTime(1986, 9, 8),
                    Age = 36,
                    Gender = GenderType.Female,
                    Biography = "Родена съм в град Своге. Имам висше образование от Педагогическия факултет (магистър) на Софийски университет \"Св. Климент Охридски\". Към момента съм начален учител в 112 ОУ \"Стоян Заимов\" и Координатор на политики по семейните въпроси в АОМ.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            bool isExistingGabriela = await userManager.Users.AnyAsync(u => u.UserName == "Габриела1");
            if (!isExistingGabriela)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Габриела1",
                    Email = "gabrielafrankfurtska@aom.bg",
                    FirstName = "Габриела",
                    SecondName = "Шенгенова",
                    LastName = "Франкфуртска",
                    BirthDate = new DateTime(2001, 5, 9),
                    Age = 21,
                    Gender = GenderType.Female,
                    Biography = "Родена съм в град София. Завършила съм Националната гимназия за древни езици и култури \"Св. Константин-Кирил Философ\", а сега уча история в Софийски университет \"Св. Климент Охридски\" и съм Координатор на политики по еврочленство и евроитеграция  в АОМ.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            bool isExistingSnaks = await userManager.Users.AnyAsync(u => u.UserName == "Снакс1");
            if (!isExistingSnaks)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Снакс1",
                    Email = "snaksmacninsky@aom.bg",
                    FirstName = "Снакс",
                    SecondName = "Аргиров",
                    LastName = "Макнински",
                    BirthDate = new DateTime(1966, 1, 1),
                    Age = 56,
                    Gender = GenderType.Male,
                    Biography = "Роден съм в град Враца. Имам висше образование от Юридическия факултет (магистър) на Софийски университет \"Св. Климент Охридски\". Работя като консултант в адвокатска кантора \"Иванчов & Партньори\". В АОМ съм Координатор на политики за външна политика.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            bool isExistingElena = await userManager.Users.AnyAsync(u => u.UserName == "Елена1");
            if (!isExistingElena)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Елена1",
                    Email = "elenarudolf@aom.bg",
                    FirstName = "Елена",
                    SecondName = "Тодорова",
                    LastName = "Рудолф",
                    BirthDate = new DateTime(1996, 4, 12),
                    Age = 26,
                    Gender = GenderType.Female,
                    Biography = "Родена съм в град Ямбол. Имам висше образование от Историческия факултет (бакалавър и магистър) на Софийски университет \"Св. Климент Охридски\". Учителка съм по \"История и цивилизация\" в НПГ по прецизна техника и оптика \"М. В. Ломоносов\", а в АОМ съм Координатор на политики за транспорт.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            bool isExistingIbrahim = await userManager.Users.AnyAsync(u => u.UserName == "Ибрахим1");
            if (!isExistingIbrahim)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Ибрахим1",
                    Email = "ibrahimkoch@aom.bg",
                    FirstName = "Ибрахим",
                    SecondName = "Али",
                    LastName = "Коч",
                    BirthDate = new DateTime(1980, 6, 24),
                    Age = 42,
                    Gender = GenderType.Male,
                    Biography = "Роден съм в град Кърджали. Имам висше образование от Техническия университет в София. Работя като техник в Ел Инс Проект-ЕООД и съм Координатор на политики за селско стопанство в АОМ.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            bool isExistingGoritsa = await userManager.Users.AnyAsync(u => u.UserName == "Горица1");
            if (!isExistingGoritsa)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Горица1",
                    Email = "goritsadubova@aom.bg",
                    FirstName = "Горица",
                    SecondName = "Букова",
                    LastName = "Дъбова",
                    BirthDate = new DateTime(1991, 3, 1),
                    Age = 31,
                    Gender = GenderType.Female,
                    Biography = "Родена съм в град Ловеч. Имам висше образование от Историческия факултет (бакалавър и магистър) на Софийски университет \"Св. Климент Охридски\". Учителка съм по \"История и цивилизация\" в Софийска професионална гимназия по туризъм, а в АОМ съм Координатор на политики за енергетика и екология.",
                    ProfilePictureURL = "..\\..\\wwwroot\\images\\images-personalPhoto.png",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "user01");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}