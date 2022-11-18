using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Administration.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace AOMForum.Web.Areas.Administration.Controllers
{
    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public IActionResult Index()
        {
            IndexViewModel viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };

            return this.View(viewModel);
        }
    }
}