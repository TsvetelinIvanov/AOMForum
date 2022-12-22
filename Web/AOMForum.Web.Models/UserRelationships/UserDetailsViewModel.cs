﻿using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.UserRelationships
{
    public class UserDetailsViewModel
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

        public IEnumerable<UserPostViewModel> Posts { get; set; } = new HashSet<UserPostViewModel>();

        public IEnumerable<UserCommentViewModel> Comments { get; set; } = new HashSet<UserCommentViewModel>();

        public IEnumerable<UserFollowerViewModel> Followers { get; set; } = new HashSet<UserFollowerViewModel>();

        public IEnumerable<UserFollowingViewModel> Followings { get; set; } = new HashSet<UserFollowingViewModel>();
    }
}