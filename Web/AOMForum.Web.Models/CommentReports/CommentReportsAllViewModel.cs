namespace AOMForum.Web.Models.CommentReports
{
    public class CommentReportsAllViewModel
    {
        public IEnumerable<CommentReportListViewModel> ReplyReports { get; set; } = new HashSet<CommentReportListViewModel>();
    }
}