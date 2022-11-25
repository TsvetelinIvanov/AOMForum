using AOMForum.Web.Models.Posts;

namespace AOMForum.Web.Models.Tags
{
    public class TagDetailsViewModel
    {
        public string? Search { get; set; }

        public TagListViewModel? Tag { get; set; }

        public IEnumerable<PostListViewModel> Posts { get; set; } = new HashSet<PostListViewModel>();
    }
}