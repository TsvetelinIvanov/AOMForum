using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using AOMForum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AOMForum.Web.Areas.Identity.Pages.Account.Manage
{
    public class EnableAuthenticatorModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<EnableAuthenticatorModel> logger;
        private readonly UrlEncoder urlEncoder;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public EnableAuthenticatorModel(
            UserManager<ApplicationUser> userManager,
            ILogger<EnableAuthenticatorModel> logger,
            UrlEncoder urlEncoder)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.urlEncoder = urlEncoder;
        }

        public string SharedKey { get; set; }

        public string AuthenticatorUri { get; set; }

        [TempData]
        public string[] RecoveryCodes { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Verification Code")]
            public string Code { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await LoadSharedKeyAndQrCodeUriAsync(user);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await this.LoadSharedKeyAndQrCodeUriAsync(user);

                return this.Page();
            }

            // Strip spaces and hypens
            var verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await this.userManager.VerifyTwoFactorTokenAsync(
                user, this.userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Input.Code", "Verification code is invalid.");
                await this.LoadSharedKeyAndQrCodeUriAsync(user);

                return this.Page();
            }

            await this.userManager.SetTwoFactorEnabledAsync(user, true);
            var userId = await this.userManager.GetUserIdAsync(user);
            this.logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", userId);

            StatusMessage = "Your authenticator app has been verified.";

            if (await this.userManager.CountRecoveryCodesAsync(user) == 0)
            {
                IEnumerable<string> recoveryCodes = await this.userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                RecoveryCodes = recoveryCodes.ToArray();
                return this.RedirectToPage("./ShowRecoveryCodes");
            }
            else
            {
                return this.RedirectToPage("./TwoFactorAuthentication");
            }
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user)
        {
            // Load the authenticator key & QR code URI to display on the form
            string unformattedKey = await this.userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await this.userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await this.userManager.GetAuthenticatorKeyAsync(user);
            }

            SharedKey = this.FormatKey(unformattedKey);

            string email = await this.userManager.GetEmailAsync(user);
            AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey);
        }

        private string FormatKey(string unformattedKey)
        {
            StringBuilder result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }

            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                this.urlEncoder.Encode("AOMForum.Web"),
                this.urlEncoder.Encode(email),
                unformattedKey);
        }
    }
}