using Microsoft.AspNetCore.Identity;
using static AOMForum.Common.DataConstants.ApplicationUser;

namespace AOMForum.Data
{
    public class IdentityOptionsProvider
    {
        public static void GetIdentityOptions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = PasswordMinLength;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = MaxFailedAccessAttemptsCount;
        }
    }
}