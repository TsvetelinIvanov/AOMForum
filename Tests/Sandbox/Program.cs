using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Common;
using AOMForum.Data.Models;
using AOMForum.Data.Repositories;
using AOMForum.Data.Seeding;
using AOMForum.Data;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Services.Data.Services;
using AOMForum.Services.Messaging;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sandbox;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.WriteLine($"{typeof(Program).Namespace} ({string.Join(" ", args)}) starts working...");
        ServiceCollection serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

        // Seed data on application startup
        using (IServiceScope serviceScope = serviceProvider.CreateScope())
        {
            ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
            new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
        }

        using (IServiceScope serviceScope = serviceProvider.CreateScope())
        {
            serviceProvider = serviceScope.ServiceProvider;

            return Parser.Default.ParseArguments<SandboxOptions>(args).MapResult(
                opts => SandboxCode(opts, serviceProvider).GetAwaiter().GetResult(),
                _ => 255);
        }
    }

    private static async Task<int> SandboxCode(SandboxOptions options, IServiceProvider serviceProvider)
    {
        Stopwatch sw = Stopwatch.StartNew();

        ISettingsService settingsService = serviceProvider.GetService<ISettingsService>();
        Console.WriteLine($"Count of settings: {settingsService.GetCount()}");

        Console.WriteLine(sw.Elapsed);
        return await Task.FromResult(0);
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .UseLoggerFactory(new LoggerFactory()));

        //services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions).AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IDbQueryRunner, DbQueryRunner>();

        // Application services
        services.AddTransient<IEmailSender, NullMessageSender>();
        services.AddTransient<ISettingsService, SettingsService>();
    }
}