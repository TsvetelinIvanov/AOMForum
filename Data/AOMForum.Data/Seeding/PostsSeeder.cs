﻿using AOMForum.Common;
using AOMForum.Data.Models.Enums;
using AOMForum.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AOMForum.Data.Seeding
{
    public class PostsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Posts.AnyAsync())
            {
                return;
            }

            string? adminYoanId = await dbContext.Users.Where(u => u.UserName == "Йоан").Select(a => a.Id).FirstOrDefaultAsync();
            if (adminYoanId == null)
            {
                return;
            }

            string? userMariaId = await dbContext.Users.Where(u => u.UserName == "Мария1").Select(u => u.Id).FirstOrDefaultAsync();
            if (userMariaId == null)
            {
                return;
            }

            string? userSnaksId = await dbContext.Users.Where(u => u.UserName == "Снакс1").Select(u => u.Id).FirstOrDefaultAsync();
            if (userSnaksId == null)
            {
                return;
            }


            int categoryId = await dbContext.Categories.Where(c => c.Name == "Демографска криза").Select(c => c.Id).FirstOrDefaultAsync();

            List<int> tagIds = await dbContext.Tags.Where(t => t.Name == "Демография" || t.Name == "Смъртност" || t.Name == "Раждаемост" || t.Name == "Емиграция" || t.Name == "Семейство" || t.Name == "Икономика" || t.Name == "Глобализация").Select(t => t.Id).ToListAsync();

            List<Post> posts = new List<Post>
            {
                new Post
                {
                    Title = "Демографска криза в България в края на ХХ и началото на ХХI век",
                    Type = PostType.Text,
                    ImageUrl = "..\\..\\wwwroot\\images\\senior-people.jpg",
                    Content = @"Отделът за населението към ООН излезе с доклад, според който през 2050 г. населението в България ще бъде с 27,9 % по-малко от това през 2015 г.
Тези тревожни данни се потвърждават от още две изследвания по темата. Прогнозите на Националния статистически институт на Република България показват, че при най-вероятния сценарий населението в България след 35 години ще бъде 5 813 550. Очакванията на Евростат са че през 2080 г. населението ще спадне с цели 37%. Това бедствие обаче ще застигне с подобен мащаб и други държави — членки на ЕС като Гърция, Португалия, Словакия и Латвия, чието население също ще намалее с около една трета.
България е с най-ниска продължителност на живота в Европейския съюз, следвана от Латвия. В страната ни хората живеят средно 74.8 години, показват данните на Националния статистически институт за периода 2016 - 2018 г. С най-висока продължителност са Испания (83.4) и Италия (83.1 години). Според експерта по демография доц. Георги Бърдаров фактите обаче наистина са такива, като абсолютният брой население и намаляването му не са основна причина за това. Проблемът е в структурите и в много високата обща смъртност. Най-големият проблем е отливът на млади хора в чужбина. Доц. Бърдаров допълва, че това е срамно и обидно, тъй като такава смъртност имат регионите с болестни пандемии и военни конфликти. В България нищо сензационно и различно не се случва, за да има такива резултати. Комплекс от фактори, като доходите, здравеопазването, начина на живот и стреса оказват влияние. Всеки един фактор в здравеопазването оказва влияние, тъй като имаме най-ниската продължителност на живот, а най-застаряващо население. Експертът смята, че политиката пряко влияе на демографията и ако държавата ни си оправи икономиката, тази демографска криза също ще се оправи.
Хората се концентрират в шест големи града, трудовият пазар е в криза, технологиите формират едно бъдеще, в което България ще има сериозни проблеми. Новото изследване на Георги Бърдаров и Надежда Илиева от БАН отново задейства познатия медиен рефлекс. Според Ивайло Дичев със силната метафора ""Демографската пустиня"" авторите успешно са активизирали националната депресия. Всички знаем, че България се обезлюдява, но образът на територия, която само след дванайсет години ще е 60% демографска пустиня (под десет жители на квадратен километър), естествено ще стресне всекиго. Ако бяха употребили някаква друга метафора, например екорезерват, щеше да звучи доста по-ободряващо. Тъжно е, че селища изчезват, защото хората все по-масово мигрират към шест големи града в страната.
Застаряването на нацията, социалните неравенства и миграцията са сред водещите фактори, които задълбочават демографската криза в страната. Това сочи изследване на учените от Българската академия на науките. Те прогнозират, че след 30 години всеки десети българин в страната ще бъде на възраст над 80 години. България няма да изчезне, уверяват учените от Института за изследване на населението и човека при БАН. Най-лошата прогноза предвижда до 2040 година населението на страната да намалее до 5,8 млн. души, ако правителството не предприеме мерки за преодоляване на кризата.",
                    
                    AuthorId = adminYoanId,
                    CategoryId = categoryId,                    
                    Tags = new List<PostTag>
                    {
                        new PostTag
                        {
                            TagId = tagIds[0]
                        },
                        new PostTag
                        {
                            TagId = tagIds[1]
                        }
                    }
                },
                new Post
                {
                    Title = "Възможности за промяна на демографската ситуация в България",
                    Type = PostType.Text,
                    ImageUrl = "..\\..\\wwwroot\\images\\family.jpg",
                    Content = @"Демографската ситуация в България въобще не е радостна и спешното подпомагане на семейството е едно от важните неща които трябва да бъдат направени за обръщане на негативната тенденция.
Според доц. Михаил Мирчев има рязка дестабилизация на семейно-брачните модели - моделите на морализирана моногамност в двуполови семейства, които са с официален брак, с раждане на деца, със съвместно и отговорно отглеждане на децата от двамата родители, живеещи в едно домакинство. Тези модели вече 2 десетилетия са подложени на бърза и масирана ерозия. Това е обективен цивилизационен процес - при съвременния манталитет на индивидуализма и мобилността. Но в същото време той е изкуствено ускоряван от социалното инженерство чрез индустрията на масовата култура и глобалната пропаганда. Медиите и масовата култура систематично и целенасочено атакуват масовата психика и съзнание чрез морално и идеологическо подценяване на тези традиционни семейно-брачни модели и пропагандирането на техни алтернативи. ""Невро-маркетингът"" и манипулирането на масовата психика и съзнание тук действат с голяма сила и последователност. Настъпателно се пропагандират и бързо се масовизират обратните модели - не-семейни, анти-брачни, без деца, еднополови партньорства, с разделено домакинство, с разделено родителство, деца със ""самотен"" родител, деца в среда на краткотрайни партньорски събирания и раздели. Семейството днес масово губи своята основна ценност - стабилността и устойчивостта си като интимна общност, продължителността и кумулативността на отношенията в него, ролята си на защитена и обгрижваща среда, в която човек намира солидарност и близост.
Елитите и високите средни класи се възпроизвеждат относително добре и пълноценно. Количествено, като брой - вероятно с тотален коефициент на плодовитост близък до нужния за просто възпроизводство (между 1,6 и 1,8). Наред с това, и качествено, те възпроизвеждат стабилно своя човешки капитал. Сред елитите и високите средни слоеве на професионалистите и управленците се възражда икономическата и социалната значимост на семейството и брака. Капиталът и властта предполагат наследяване и предаване на близки хора, ценностно осмисляне и мотивиране чрез близките, децата и внуците, чрез родовото продължаване. Същата потребност има сред професионалистите - те имат привилегирован обществен статус, който сами си извоюват и удържат чрез своите знания и квалификация. Сред тези социални слоеве и общности при 2-ри и 3-ти брак има мотивация за раждане на още свои деца - материалните и статусните условия за обезпечаването им са налични. Проблемите с достатъчната раждаемост сред тези слоеве на върха на пирамидата идват от свръх-еманципацията на жените -професионалистки и с висока кариера, жените със самостоятелни светски и властови амбиции. Всичко това за много от тях прави невъзможно създаването и удържането на устойчиво семейство и партньорство. Още, поради отлагането раждането на първото дете, изпускането на естествения момент, което в края на краищата води масово до еднодетство, при това често на рисково висока възраст - за второ дете просто не остава време, нито мотивация и жизнена сила. Проблем е и възприемането на стилове на живот и кариера, при които отдаването за достатъчно дълъг период от време на пълноценно майчинство става трудно осъществимо. Това само по себе си е детеродна спирачка, превръщана от такива жени във философия, която фаворизира кариерата като екзистенциален заместител на майчинството и семейния уют. Сред тези слоеве като спирачка действат още: хомосексуалната мода (главно сред мъжете, които са нужни на жените като надеждни партньори); показният хедонизъм, в логиката на който семейството се възприема като досадно ограничаване на личната свобода и спонтанност.
При средните и ниските слоеве на средната класа има обратна ситуация. В условията на глобализация тези слоеве масово са засегнати от дестабилизиране, от процентно намаляване, от относително обедняване и от декласиране, масовите средни слоеве на средната класа в най-голяма степен свиха своята детеродната норма - и субективно като нагласи, и обективно като житейска реалност, също така от техните професионални и граждански среди е най-голямата маса на емигриращите и оставащите да живеят в чужбина. Именно сред тези слоеве поради емиграцията са най-голям брой разделените семействата, в които се скъсва родовата приемственост и възпроизводство - старите остават в самота и с чувството за изоставеност, а младите се атомизират, борейки се за оцеляване и интегриране във външна и често негостоприемна чужда културна среда (индивидуалистична и прагматична като базова ценностна нагласа). Кумулирането на ефекта от горните три демографски фактори (свита и забавена раждаемост, масова емиграция, страх от бъдещето и детеродна демотивация) именно сред тези слоеве и общности е най-значимо. По този начин се осъществява деформиране на социалната структура на обществото като цяло. Още повече, че именно младите поколения от тези слоеве в най-голяма степен се поддават и възприемат пропагандираните от масовата култура и медиите алтернативни модели за партньорство и семейство, за бездетство, за разделено и самотно живеене, за алтернативен секс и хедонистични практики, които намаляват детеродната способност и способността за устойчиво семейно и родителско партньорство - и мъжко, и женско. Има нужда демографите и социолозите да рисуват и прогнозират възпроизводствените процеси в хоризонталната специфика на основните социални слоеве, а не традиционно само чрез усреднените коефициенти и показатели. Има драстични разлики - количествени и качествени - между реалните процеси на възпроизводство сред основните стратификационни слоеве. Още, защото различните слоеве имат различна значимост за общото развитие на икономиката и производителния капацитет на обществото и държавата, както и за съхраняването на българската нация. 
Ниските социализирани слоеве - работнически, чиновнически, земеделски, обслужващи - също имат относително добро демографско възпроизводство. Проблемът сред тези слоеве не е толкова в нежелаенето на 2-ро дете, а в натрупването на здравословни проблеми и трудности при забременяване, в многото принудени аборти и вредите от тях, в поразяването на значителни части от младите поколения в тези слоеве от алкохолизъм и наркомании, от модерни женски стилове на живот, въз основа на вулгарност и рисков промискуитет, както и от масово поразяване на младите мъже от чувство за непълноценност, бедност и комплексираност пред жените. Големият проблем при тях е качествен - намалява техният човешки капитал. В тези слоеве и общности бързо се снижава степента и качеството на образоваността, свива се и се примитивизира културността им, натрупва се масова политическа апатия или агресивност, разколебава се базовият им рефлекс към традиционна идентичност и обществена интегрираност. Сред тези слоеве и общности има голяма структурна и регионална безработица. Масово се получават ниски доходи, поради което се поддържа стандарт на живот на оцеляването. Това ерозира в голям обем тези слоеве и общности, снижава жизнения им тонус, дезинтегрира ги от държавата и ценностите на обществото, изтласква ги към асоциалност и лумпенизация. Последното означава излизане извън активната работна сила в държавата, преминаване към стратегии и практики на паразитизъм и консумативност, в края на краищата към снижаване на обществената им производителност общо като граждански слой и общности, вкл. критично свиване на мотивацията им за раждане на деца и възпроизводство.
В зоната на крайната бедност, която закономерно прераства и в маргиналност и аутсайдерство, обратно на всички горни стратификационни слоеве, има в пъти по-висока норма на раждаемост - субективна, и още по-висока като житейска реалност. Вместо намаляване и свиване тук става разрастване - и като брой, и като процент сред цялото население. Вместо интегриране и цивилизоване, тук - поради нарастването на броя, и поради компактността на общностите и примитивните социални отношения в тях, и поради възприемането на асоциални модели за публично поведение, и поради обричането на децата не само на бедност, но и на малограмотност процесът е обратен. Не цивилизоване, а връщане назад към патриархално-феодални порядки, към примитивност и природна първичност. Това в класификационната схема на Алвин Тофлър е или връщане към ""първата вълна"", или още по-назад към предисторията. Погледнато отново откъм качеството на човешкия капитал бързо се увеличава делът на децата и младежите (по закон бъдещи пълноправни граждани), които са затиснати от заобикалящата ги масова малограмотност и неграмотност, от масова лумпенизация и асоциалност, от липса на социализиращ родителски пример. Това е бързо разрастващо се население, което е изпаднало и остава под нужния минимум от трудови навици и базови социални умения, от желание за интегриране и готовност за обществена дисциплина, без нужната мотивация и собствено усилие за обществена интеграция. От чисто количествена и статистическа гл.т. тази рязко по-висока слоева раждаемост изкуствено надува общото число и коефициенти на раждаемостта в България. Това е заблуждаващо. Ако се отчита раждаемостта по слоеве, тогава ясно ще се види, колко катастрофално ниска е раждаемост и възпроизводство при образованите, производителните и интегрираните слоеве, при средните класи, но и също така колко застрашително висока е раждаемостта сред аутсайдерските слоеве.
Докато сред образованите общности и средните слоеве има бързо покачване на възрастта на първото раждане, става дума за така нар. ""отложена раждаемост"" вече масово до и над 30 години на жената и последващата я еднодетност, то при най-ниските и маргиналните слоеве, обратно, има масова ""ранна детска"" и множествена раждаемост. Така детеродната норма не само се различава в пъти, но се получава и едно изпреварване - вече с цяло едно поколение - на ниско-статусната раждаемост спрямо средно- и високо-статусната раждаемост. Това следва да бъде специален разрез при прогнозирането, особено в средно и дългосрочна демографска перспектива. Напр. важно е да се знае, че около 2050 г. населението на България ще се стопи до около 5 млн. души (при сегашна база). Но вероятно още по-важно е да се каже и каква пропорция бихме имали тогава между образовани и социализирани слоеве срещу малограмотни и аутсайдерски, колко ще са българите срещу дезинтегрираните малцинства, кои общности ще са затихващо-остарели и кои ще са жизнено-младежки. Това са съдбоносни за бъдещето на държавата и на нацията ни структурни разрези.
Погледнато икономически, това поколенческо разминаване между основните слоеве - високо-производителни и ниско-производителни, всъщност, запушва икономиката, спира развитието й. Просто защото се оказва, че икономиката трябва дълго да изчаква новите качествени поколения работна и гражданска сила, а междувременно се задръства от ниско-културна, ниско-производителна и трудно-интегрируема човешка маса.Негодните за работа или само за ниско-производителна работа стават все по-голяма тежест - не само когато не работят (получават нарастващо социално подпомагане и издръжка), но и когато работят, защото не са годни за производителен труд в условията на детайлна регламентация и прецизна организираност (""втората вълна"" по Тофлър), нито за ефективна самоорганизация за производителен и творчески дистанционен труд (""третата вълна"" по Тофлър). Не е важно само количеството на работната сила, но и нейното качество. Работодателите приплакват, че не могат да намерят годни и производителни работници, служители и специалисти. Проблемът е вече остър, вече е осъзнат. Търси се решение. Но при това търсене има и една голяма опасност. Може този проблем допълнително да бъде утежнен, ако дефицитът от работна сила, се тръгне да бъде запълван чрез внос на външна работна сила. Проблемът е ако външната работна сила така се подбира, че да се включи към нашите ниски и маргинални слоеве, а не към нашите интегрирани и производителни среди и елитни общности и слоеве. Ако направим това, пак ще удвоим вече видимия си вътрешен проблем. Българската икономика се нуждае от качествена работна сила - образована и квалифицирана, културна и възпитана, социализирана и интегрирана. Нужни са хора, преди всичко млади хора, но не какви да е, а образовани, социализирани, интегрирани. Това е пряката връзка между демографските процеси и възходящата или запушената перспектива за икономиката ни.
От разгледаното до тук става ясно, че семейната стратегия трябва да бъде внимателно съобразена с различните социални слоеве и да бъде насочена към най-засегната част от населението - долните слоеве на средната класа и работниците. Това са и хорота които до голяма степен осигуряват бюджетните приходи и именно там трябва да се наблегне при провеждането на семейни политики. Трябва коренно да се промени досегашното отношение на държавата към различните семейни общности, като например увеличаване на използването на данъчни облекчения за млади семейства с деца вместо директното даване на пари.",
                    AuthorId = userMariaId,
                    CategoryId = categoryId,                    
                    Tags = new List<PostTag>
                    {
                        new PostTag
                        {
                            TagId = tagIds[0]
                        },
                        new PostTag
                        {
                            TagId = tagIds[2]
                        },
                        new PostTag
                        {
                            TagId = tagIds[3]
                        },
                        new PostTag
                        {
                            TagId = tagIds[4]
                        },
                        new PostTag
                        {
                            TagId = tagIds[5]
                        },
                        new PostTag
                        {
                            TagId = tagIds[6]
                        }
                    }
                },
                new Post
                {
                    Title = "Отражение на икономика върху демографското развитие",
                    Type = PostType.Text,
                    ImageUrl = "..\\..\\wwwroot\\images\\harvest-.jpg",
                    Content = @"Демографският срив е по-скоро проблем за пазара на труда. 
Георги Бърдаров е сред малкото, които обясняват какво е демографски преход: онзи необратим процес, при който раждаемостта спада успоредно с нарастването на стандарта. Навсякъде, във всички общества. И в това няма особена трагедия, тъкмо напротив – то е знак за развитост на обществото. Раждаемостта в България от 1,53 на жена съвсем не е ниска в сравнение с другите европейски страни. Проблемът е високата смъртност и хроничната емиграция. Разбира се, стимулите за раждане на повече деца са хубаво нещо. Докладът обаче припомня един факт, който сякаш не интересува българските медии: въпреки сериозните пари, които изплащат за раждането на дете в Русия и Унгария, резултатът се измерва в едни пренебрежими десети от процента, а прирастът си остава все така отрицателен. Знаем също така, че родените сега са инвестиция за след 20 години – и вероятно ще растат в споменатата „пустиня“. Идеята да върнем българските емигранти, като им дадем по-високи заплати, е медийно клише, което авторите на изследването сякаш не са подложили на критичен анализ. Ако работодателите имаха пари, щяха да дават. Но за да имат, трябва да се завърти производството, а за това пък няма хора. Виж, ако държавата реши да подпомага завръщащите се с жилища, оземляване, кредити - това вече е друг разговор.
Голям демографски въпрос е и дали технологическото развитие няма да направи човешките същества излишни. Не само шофьори, полицаи и секретарки, но и лекари, инженери, юристи. Ако утре всичко ще върши изкуственият интелект, защо изобщо да се тревожим, че територията се обезлюдява? Ще си караме четвъртата възраст пред телевизорите и някакъв робот ще ни сменя памперсите. А наоколо 60% чиста природа. Според Ивайло Дичев от хора ще има нужда – роботите няма да направят излишни само определени групи вътре в страните – те ще маргинализират цели държави, които не успяват да се адаптират към новата икономика, а една такава държава рискува да стане България.",
                    AuthorId = userSnaksId,
                    CategoryId = categoryId,                    
                    Tags = new List<PostTag>
                    {
                        new PostTag
                        {
                            TagId = tagIds[0]
                        },
                        new PostTag
                        {
                            TagId = tagIds[3]
                        },
                        new PostTag
                        {
                            TagId = tagIds[5]
                        }
                    }
                }
            };

            await dbContext.Posts.AddRangeAsync(posts);
        }
    }
}