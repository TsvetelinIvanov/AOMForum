using System.ComponentModel.DataAnnotations;
using AOMForum.Data.Models;
using AOMForum.Services.Mapping;
using static AOMForum.Common.DisplayNames.Category;

namespace AOMForum.Web.Models.Home
{
    public class IndexCategoryListingViewModel : IMapFrom<Category>
    {
        [Display(Name = DisplayName)]
        public string? Name { get; init; }

        [Display(Name = DisplayDescription)]
        public string? Description { get; init; }

        public string? ImageURL { get; init; }

        [Display(Name = DisplayPostsCount)]
        public int PostsCount { get; init; }

        public string? URL => $"/f/{this.Name?.Replace(' ', '-')}";
    }
}