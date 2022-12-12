﻿using AOMForum.Data.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AOMForum.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginWithRecoveryCodeModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginWithRecoveryCodeModel> logger;

        public LoginWithRecoveryCodeModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginWithRecoveryCodeModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [BindProperty]
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Recovery Code")]
            public string RecoveryCode { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            ApplicationUser user = await this.signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            this.ReturnUrl = returnUrl;

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            ApplicationUser user = await this.signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            string recoveryCode = Input.RecoveryCode.Replace(" ", string.Empty);

            Microsoft.AspNetCore.Identity.SignInResult result = await this.signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                this.logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);

                return this.LocalRedirect(returnUrl ?? Url.Content("~/"));
            }
            if (result.IsLockedOut)
            {
                this.logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);

                return this.RedirectToPage("./Lockout");
            }
            else
            {
                this.logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
                this.ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");

                return this.Page();
            }
        }
    }
}