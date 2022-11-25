using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Home
{
    public class HomeAdminViewModel
    {
        public string? Id { get; init; }

        [Display(Name = DisplayUserName)]
        public string? UserName { get; init; }

        [Display(Name = DisplayEmail)]
        public string? Email { get; init; }

        public string? ProfilePictureURL { get; init; }
    }
}