using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models;
using AOMForum.Web.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AOMForum.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> logger;
        private readonly IUsersService usersService;

        public HomeController(ILogger<HomeController> logger, IUsersService usersService)
        {
            this.logger = logger;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel viewModel = await this.usersService.GetHomeViewModelAsync();

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}