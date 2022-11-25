namespace AOMForum.Web.Models.Home
{
    public class IndexViewModel
    {
        public IEnumerable<IndexCategoryListingViewModel> Categories { get; set; } = new HashSet<IndexCategoryListingViewModel>();
    }
}