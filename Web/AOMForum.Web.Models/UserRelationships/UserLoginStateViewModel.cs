using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.UserRelationships
{
    public class UserLoginStateViewModel
    {
        public string? Id { get; init; }

        [Display(Name = DisplayUserName)]
        public string? UserName { get; init; }

        [Display(Name = DisplayProfilePictureURL)]
        public string? ProfilePictureURL { get; init; }
    }
}