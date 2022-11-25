using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Users
{
    public class UserFollowersViewModel
    {
        public string? Id { get; init; }

        [Display(Name = DisplayUserName)]
        public string? UserName { get; init; }

        public string? ProfilePictureURL { get; init; }

        [Display(Name = DisplayCoilsCount)]
        public int CoilsCount { get; init; }

        [Display(Name = DisplayUserCommentsCount)]
        public int UserCommentsCount { get; init; }

        [Display(Name = DisplayUserVotesCount)]
        public int UserVotesCount { get; init; }
    }
}