using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Home;

namespace AOMForum.Web.Models.Home
{
    public class HomeViewModel
    {
        [Display(Name = DisplayPostsCount)]
        public int PostsCount { get; init; }

        [Display(Name = DisplayUsersCount)]
        public int UsersCount { get; init; }

        [Display(Name = DisplayVotesCount)]
        public int VotesCount { get; init; }

        public IEnumerable<HomeAdminViewModel> Admins { get; set; } = new HashSet<HomeAdminViewModel>();
    }
}