namespace AOMForum.Web.Models.Categories
{
    public class CategoriesAllViewModel
    {
        public string? Search { get; set; }

        public IEnumerable<CategoryListViewModel> Categories { get; set; } = new HashSet<CategoryListViewModel>();
    }
}