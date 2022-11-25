namespace AOMForum.Web.Models.PostReports
{
    public class PostReportsAllViewModel
    {
        public IEnumerable<PostReportListViewModel> PostReports { get; set; } = new HashSet<PostReportListViewModel>();
    }
}