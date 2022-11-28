using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Category;

namespace AOMForum.Web.Models.Categories
{
    public class CategoriesAllViewModel
    {
        [Display(Name = DisplaySearch)]
        public string? Search { get; set; }

        public IEnumerable<CategoryListViewModel> Categories { get; set; } = new HashSet<CategoryListViewModel>();
    }
}