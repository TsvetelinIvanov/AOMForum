using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Settings;
using Microsoft.AspNetCore.Mvc;

namespace AOMForum.Web.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly ISettingsService settingsService;

        private readonly IDeletableEntityRepository<Setting> settingsRepository;

        public SettingsController(ISettingsService settingsService, IDeletableEntityRepository<Setting> settingsRepository)
        {
            this.settingsService = settingsService;
            this.settingsRepository = settingsRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<SettingViewModel> settings = this.settingsService.GetAll<SettingViewModel>();
            SettingsListViewModel model = new SettingsListViewModel { Settings = settings };

            return this.View(model);
        }

        public async Task<IActionResult> InsertSetting()
        {
            Random random = new Random();
            Setting setting = new Setting { Name = $"Name_{random.Next()}", Content = $"Value_{random.Next()}" };

            await this.settingsRepository.AddAsync(setting);
            await this.settingsRepository.SaveChangesAsync();

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}