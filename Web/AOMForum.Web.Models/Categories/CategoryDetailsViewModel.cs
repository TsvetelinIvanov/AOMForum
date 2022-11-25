namespace AOMForum.Web.Models.Categories
{
    public class CategoryDetailsViewModel
    {
        public string? Search { get; set; }

        public CategoryListViewModel? Category { get; set; }

        public IEnumerable<PostInCategoryViewModel> Posts { get; set; } = new HashSet<PostInCategoryViewModel>();
    }
}