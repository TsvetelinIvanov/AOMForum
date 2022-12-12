using AOMForum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AOMForum.Web.Areas.Identity.Pages.Account.Manage
{
    public class GenerateRecoveryCodesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<GenerateRecoveryCodesModel> logger;

        public GenerateRecoveryCodesModel(
            UserManager<ApplicationUser> userManager,
            ILogger<GenerateRecoveryCodesModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        [TempData]
        public string[] RecoveryCodes { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            bool isTwoFactorEnabled = await this.userManager.GetTwoFactorEnabledAsync(user);
            if (!isTwoFactorEnabled)
            {
                string userId = await this.userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{userId}' because they do not have 2FA enabled.");
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            bool isTwoFactorEnabled = await this.userManager.GetTwoFactorEnabledAsync(user);
            string userId = await this.userManager.GetUserIdAsync(user);
            if (!isTwoFactorEnabled)
            {
                throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{userId}' as they do not have 2FA enabled.");
            }

            IEnumerable<string> recoveryCodes = await this.userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            RecoveryCodes = recoveryCodes.ToArray();

            this.logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", userId);
            this.StatusMessage = "You have generated new recovery codes.";

            return this.RedirectToPage("./ShowRecoveryCodes");
        }
    }
}