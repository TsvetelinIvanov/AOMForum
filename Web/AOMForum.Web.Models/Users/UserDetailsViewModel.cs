using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Users
{
    public class UserDetailsViewModel
    {
        public string? Id { get; init; }

        [Display(Name = DisplayUserName)]
        public string? UserName { get; init; }

        public string? ProfilePictureURL { get; init; }

        [Display(Name = DisplayUserVotesCount)]
        public int UserVotesCount { get; init; }

        public bool IsFollowed { get; init; }

        [Display(Name = DisplayFollowersCount)]
        public int FollowersCount { get; init; }

        [Display(Name = DisplayFollowingsCount)]
        public int FollowingsCount { get; init; }

        public IEnumerable<UserCoilViewModel> Threads { get; set; } = new HashSet<UserCoilViewModel>();

        public IEnumerable<UserCommentViewModel> Replies { get; set; } = new HashSet<UserCommentViewModel>();

        public IEnumerable<UserFollowersViewModel> Followers { get; set; } = new HashSet<UserFollowersViewModel>();

        public IEnumerable<UserFollowingsViewModel> Following { get; set; } = new HashSet<UserFollowingsViewModel>();
    }
}