using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Tag;

namespace AOMForum.Web.Models.Tags
{
    public class TagListViewModel
    {
        public int Id { get; init; }

        [Display(Name = DisplayName)]
        public string? Name { get; init; }

        [Display(Name = DisplayPostsCount)]
        public int PostsCount { get; init; }
    }
}