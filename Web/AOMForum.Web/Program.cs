using AOMForum.Data;
using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Common;
using AOMForum.Data.Models;
using AOMForum.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AOMForum.Services.Messaging;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Services.Data.Services;
using Microsoft.AspNetCore.Mvc;
using AOMForum.Services.Mapping;
using AOMForum.Web.Models;
using System.Reflection;
using AOMForum.Data.Seeding;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });//

builder.Services.AddControllersWithViews().AddMvcOptions(options => 
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
})
    .AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();

// Data repositories
builder.Services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IDbQueryRunner, DbQueryRunner>();

// Application services
builder.Services.AddTransient<IEmailSender, NullMessageSender>();
builder.Services.AddTransient<ICategoriesService, CategoriesService>();
builder.Services.AddTransient<ITagsService, TagsService>();
builder.Services.AddTransient<IPostsService, PostsService>();
builder.Services.AddTransient<IPostReportsService, PostReportsService>();
builder.Services.AddTransient<IPostVotesService, PostVotesService>();
builder.Services.AddTransient<ICommentsService, CommentsService>();
builder.Services.AddTransient<ICommentReportsService, CommentReportsService>();
builder.Services.AddTransient<ICommentVotesService, CommentVotesService>();
builder.Services.AddTransient<IRelationshipsService, RelationshipsService>();
builder.Services.AddTransient<IMessagesService, MessagesService>();
builder.Services.AddTransient<ISettingsService, SettingsService>();

WebApplication app = builder.Build();

AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);//

// Seed data on application
//using (IServiceScope serviceScope = app.Services.CreateScope())
//{
//    ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//    if (app.Environment.IsDevelopment())
//    {
//        dbContext.Database.Migrate();
//    }

//    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();//
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();//

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();