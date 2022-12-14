using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Category;

namespace AOMForum.Web.Models.Categories
{
    public class CategoryDetailsViewModel
    {
        public int Id { get; init; }

        [Display(Name = DisplayName)]
        public string? Name { get; init; }

        [Display(Name = DisplayDescription)]
        public string? Description { get; init; }

        [Display(Name = DisplayImageUrl)]
        public string? ImageUrl { get; init; }

        [Display(Name = DisplayPostsCount)]
        public int PostsCount { get; init; }

        public IEnumerable<PostInCategoryViewModel> Posts { get; set; } = new HashSet<PostInCategoryViewModel>();
    }
}