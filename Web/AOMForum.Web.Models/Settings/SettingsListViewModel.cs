namespace AOMForum.Web.Models.Settings
{
    public class SettingsListViewModel
    {
        public IEnumerable<SettingViewModel> Settings { get; set; } = new List<SettingViewModel>();
    }
}