namespace AOMForum.Web.Models.Posts
{
    public class PostsAllViewModel
    {
        public IEnumerable<PostListViewModel> Posts { get; set; } = new HashSet<PostListViewModel>();

        public string? Search { get; set; }

        public int FollowingsCount { get; init; }

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