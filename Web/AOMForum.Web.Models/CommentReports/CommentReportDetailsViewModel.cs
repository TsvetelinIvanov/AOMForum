using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.GlobalConstants;
using static AOMForum.Common.DisplayNames.CommentReport;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.CommentReports
{
    public class CommentReportDetailsViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public CommentReportDetailsViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(IFrameTag);
        }

        public int Id { get; set; }

        [Display(Name = DisplayContent)]
        public string? Content { get; set; }

        [Display(Name = DisplayContent)]
        public string SanitizedContent => this.sanitizer.Sanitize(this.Content ?? string.Empty);

        [Display(Name = DisplayCreatedOn)]
        public string? CreatedOn { get; set; }

        public string? AuthorId { get; set; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; set; }

        [Display(Name = DisplayProfilePictureURL)]
        public string? AuthorProfilePictureURL { get; set; }

        public int CommentId { get; set; }

        [Display(Name = DisplayCommentContent)]
        public string? CommentContent { get; set; }

        [Display(Name = DisplayCommentContent)]
        public string SanitizedCommentContent => this.sanitizer.Sanitize(this.CommentContent ?? string.Empty);
    }
}