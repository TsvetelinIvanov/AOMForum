using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static AOMForum.Common.DisplayNames.Category;

namespace AOMForum.Web.Models.Categories
{
    public class CategoryDeleteModel
    {
        public int Id { get; init; }

        [Display(Name = DisplayName)]
        public string? Name { get; init; }

        [Display(Name = DisplayDescription)]
        public string? Description { get; init; }

        public string? ImageURL { get; init; }

        [Display(Name = DisplayPostsCount)]
        public int PostsCount { get; init; }
    }
}