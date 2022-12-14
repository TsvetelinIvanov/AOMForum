using AOMForum.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AOMForum.Web.Infrastructure.Attributes;
using AOMForum.Data.Models.Enums;
using AOMForum.Services.Data.Interfaces;
using static AOMForum.Common.DataErrorMessages;
using static AOMForum.Common.DataConstants.ApplicationUser;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly IUsersService usersService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IUsersService usersService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.usersService = usersService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        //public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = RequiredErrorMessage)]
            [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength, ErrorMessage = StringLengthErrorMessage)]
            [Display(Name = DisplayUserName)]
            public string? Username { get; set; }

            [Required(ErrorMessage = RequiredErrorMessage)]
            [EmailAddress(ErrorMessage = EmailErrorMessage)]
            [StringLength(EmailMaxLength, MinimumLength = EmailMinLength, ErrorMessage = StringLengthErrorMessage)]
            [Display(Name = DisplayEmail)]
            public string? Email { get; set; }

            [Required(ErrorMessage = RequiredErrorMessage)]
            [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength, ErrorMessage = StringLengthErrorMessage)]
            [Display(Name = DisplayFirstName)]
            public string? FirstName { get; set; }

            [Required(ErrorMessage = RequiredErrorMessage)]
            [StringLength(SecondNameMaxLength, MinimumLength = SecondNameMinLength, ErrorMessage = StringLengthErrorMessage)]
            [Display(Name = DisplaySecondName)]
            public string? SecondName { get; set; }

            [Required(ErrorMessage = RequiredErrorMessage)]
            [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength, ErrorMessage = StringLengthErrorMessage)]
            [Display(Name = DisplayUserName)]
            public string? LastName { get; set; }
            
            [DataType(DataType.Date)]            
            [MinAgeRestriction(AgeMinValue, ErrorMessage = AgeRestrictionErrorMessage)]
            [Display(Name = DisplayBirthDate)]
            public DateTime BirthDate { get; set; }            

            [Required(ErrorMessage = RequiredErrorMessage)]            
            [Display(Name = DisplayGender)]
            public string? Gender { get; set; }

            [Required(ErrorMessage = RequiredErrorMessage)]
            [StringLength(BiographyMaxLength, MinimumLength = BiographyMinLength, ErrorMessage = StringLengthErrorMessage)]
            [Display(Name = DisplayBiography)]
            public string? Biography { get; set; }
            
            [Display(Name = DisplayProfilePictureURL)]
            public string? ProfilePictureURL { get; set; }

            [Required(ErrorMessage = RequiredErrorMessage)]
            [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = StringLengthErrorMessage)]
            [DataType(DataType.Password)]
            [Display(Name = DisplayPassword)]
            public string? Password { get; set; }

            [Compare(nameof(Password), ErrorMessage = ConfirmPasswordErrorMessage)]
            [DataType(DataType.Password)]
            [Display(Name = DisplayConfirmPassword)]            
            public string? ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            //this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            //this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (this.ModelState.IsValid)
            {
                GenderType genderType = GenderType.Unknown;
                string? gender = this.Input.Gender;
                if (gender == null)
                {
                    genderType = GenderType.Unknown;
                }
                else if (gender == "Мъж" || gender == "мъж" || gender == "мъжки")
                {
                    genderType = GenderType.Male;
                }
                else if (gender == "Жена" || gender == "жена" || gender == "женски")
                {
                    genderType = GenderType.Female;
                }
                else if (gender == "Друг" || gender == "друг" || gender == "друго")
                {
                    genderType = GenderType.Other;
                }
                else
                {
                    this.ModelState.AddModelError(nameof(Input.Gender), InvalidGenderErrorMessage);

                    return this.Page();
                }

                int age = DateTime.Now.Year - this.Input.BirthDate.Year;
                if (age < AgeMinValue || age > AgeMaxValue)
                {
                    this.ModelState.AddModelError(nameof(Input.BirthDate), string.Format(AgeErrorMessage, AgeMinValue, AgeMaxValue, DateTime.Now.Year - AgeMaxValue));

                    return this.Page();
                }

                bool isUsernameUsed = await this.usersService.IsUsernameUsedAsync(this.Input.Username);
                if (isUsernameUsed)
                {
                    this.ModelState.AddModelError(nameof(Input.Username), ExistingUsernameErrorMessage);

                    return this.Page();
                }

                ApplicationUser user = new ApplicationUser 
                { 
                    UserName = this.Input.Username,
                    Email = this.Input.Email,
                    FirstName = this.Input.FirstName,
                    SecondName = this.Input.SecondName,
                    LastName = this.Input.LastName,
                    BirthDate = this.Input.BirthDate,
                    Age = age,
                    Gender = genderType,
                    Biography = this.Input.Biography,
                    ProfilePictureURL = this.Input.ProfilePictureURL
                };

                IdentityResult result = await this.userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    //string code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //string? callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await this.emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl = returnUrl });
                    //}
                    //else
                    //{
                    //    await this.signInManager.SignInAsync(user, isPersistent: false);

                    //    return this.LocalRedirect(returnUrl);
                    //}
                    await this.signInManager.SignInAsync(user, isPersistent: false);

                    return this.LocalRedirect(returnUrl);
                }

                foreach (IdentityError error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}