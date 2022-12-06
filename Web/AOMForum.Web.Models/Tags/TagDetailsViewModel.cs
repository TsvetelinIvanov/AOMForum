using AOMForum.Web.Models.Posts;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Tag;

namespace AOMForum.Web.Models.Tags
{
    public class TagDetailsViewModel
    {
        public string? Search { get; set; }

        public int Id { get; init; }

        [Display(Name = DisplayName)]
        public string? Name { get; init; }

        [Display(Name = DisplayPostsCount)]
        public int PostsCount { get; init; }

        public IEnumerable<PostListViewModel> Posts { get; set; } = new HashSet<PostListViewModel>();
    }
}