using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.GlobalConstants;
using static AOMForum.Common.DataConstants.CommentReport;
using static AOMForum.Common.DisplayNames.CommentReport;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.CommentReports
{
    public class CommentReportDeleteModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public CommentReportDeleteModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(IFrameTag);
        }

        public int Id { get; init; }

        [Display(Name = DisplayContent)]
        public string? Content { get; set; }

        [Display(Name = DisplayShortContent)]
        public string ShortContent
        {
            get
            {
                string sanitizedContent = this.sanitizer.Sanitize(this.Content ?? string.Empty);

                return this.Content?.Length > ShortContentMaxLength ? sanitizedContent[..ShortContentMaxLength] + "..." : sanitizedContent;
            }
        }

        [Display(Name = DisplayCreatedOn)]
        public string? CreatedOn { get; set; }

        public int CommentId { get; set; }

        [Display(Name = DisplayCommentContent)]
        public string? CommentContent { get; set; }

        [Display(Name = DisplayCommentShortContent)]
        public string ShortCommentContent
        {
            get
            {
                string sanitizedCommentContent = this.sanitizer.Sanitize(this.CommentContent ?? string.Empty);

                return this.CommentContent?.Length > ShortContentMaxLength ? sanitizedCommentContent.Substring(0, ShortContentMaxLength) + "..." : sanitizedCommentContent;
            }
        }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; set; }

        [Display(Name = DisplayProfilePictureURL)]
        public string? AuthorProfilePictureURL { get; set; }
    }
}