using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.UserRelationships
{
    public class UserFollowingViewModel
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

        [Display(Name = DisplayUserVotesCount)]
        public int VotesCount { get; init; }
    }
}