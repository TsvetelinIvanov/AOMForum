using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.GlobalConstants;
using static AOMForum.Common.DisplayNames.PostReport;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.PostReports
{
    public class PostReportDetailsViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public PostReportDetailsViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(IFrameTag);
        }

        public int Id { get; init; }

        [Display(Name = DisplayContent)]
        public string? Content { get; init; }

        [Display(Name = DisplayContent)]
        public string? SanitizedContent => this.sanitizer.Sanitize(this.Content ?? string.Empty);

        [Display(Name = DisplayCreatedOn)]
        public string? CreatedOn { get; init; }

        public int PostId { get; init; }

        [Display(Name = DisplayPostTitle)]
        public string? PostTitle { get; init; }

        public string? AuthorId { get; init; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }

        public string? AuthorProfilePictureURL { get; init; }
    }
}