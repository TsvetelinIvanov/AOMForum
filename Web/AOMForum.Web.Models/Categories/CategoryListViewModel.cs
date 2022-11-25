using AOMForum.Data.Models;
using AOMForum.Services.Mapping;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Category;

namespace AOMForum.Web.Models.Categories
{
    public class CategoryListViewModel : IMapFrom<Category>
    {
        public int Id { get; init; }

        [Display(Name = DisplayName)]
        public string? Name { get; init; }

        [Display(Name = DisplayDescription)]
        public string? Description { get; init; }

        public string? ImageURL { get; init; }

        [Display(Name = DisplayPostsCount)]
        public int PostsCount { get; init; }

        //public int CurrentPage { get; init; }

        //public int PagesCount { get; init; }

        public IEnumerable<PostInCategoryViewModel> Posts { get; set; } = new HashSet<PostInCategoryViewModel>();
    }
}