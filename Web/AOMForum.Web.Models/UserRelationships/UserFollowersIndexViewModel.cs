using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.UserRelationships
{
    public class UserFollowersIndexViewModel
    {
        public string? Id { get; init; }

        [Display(Name = DisplayUserName)]
        public string? UserName { get; init; }

        [Display(Name = DisplayProfilePictureURL)]
        public string? ProfilePictureURL { get; init; }

        [Display(Name = DisplayPostsCount)]
        public int PostsCount { get; init; }

        [Display(Name = DisplayUserCommentsCount)]
        public int CommentsCount { get; init; }
        
        public bool IsFollowed { get; init; }

        [Display(Name = DisplayFollowersCount)]
        public int FollowersCount { get; init; }

        [Display(Name = DisplayFollowingsCount)]
        public int FollowingsCount { get; init; }

        public IEnumerable<UserFollowerViewModel> Followers { get; set; } = new HashSet<UserFollowerViewModel>();
    }
}