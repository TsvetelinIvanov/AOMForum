namespace AOMForum.Web.Models.Tags
{
    public class TagsAllViewModel
    {
        public string? Search { get; init; }

        public IEnumerable<TagListViewModel> Tags { get; set; } = new HashSet<TagListViewModel>();

        public int PageIndex { get; set; }

        public int TotalPagesCount { get; set; }

        public int NextPage
        {
            get
            {
                if (this.PageIndex >= this.TotalPagesCount)
                {
                    return 1;
                }

                return this.PageIndex + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (this.PageIndex <= 1)
                {
                    return this.TotalPagesCount;
                }

                return this.PageIndex - 1;
            }
        }
    }
}