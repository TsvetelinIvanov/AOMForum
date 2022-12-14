using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.GlobalConstants;
using static AOMForum.Common.DataConstants.PostReport;
using static AOMForum.Common.DisplayNames.PostReport;

namespace AOMForum.Web.Models.PostReports
{
    public class PostReportListViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public PostReportListViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(IFrameTag);
        }

        public int Id { get; init; }

        [Display(Name = DisplayContent)]
        public string? Content { get; init; }

        [Display(Name = DisplayShortContent)]
        public string? ShortContent
        {
            get
            {
                string sanitizedContent = this.sanitizer.Sanitize(this.Content ?? string.Empty);

                return this.Content?.Length > ShortContentMaxLength ? sanitizedContent[..ShortContentMaxLength] + "..." : sanitizedContent;
            }
        }

        [Display(Name = DisplayCreatedOn)]
        public string? CreatedOn { get; init; }

        [Display(Name = DisplayPostTitle)]
        public string? PostTitle { get; init; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }

        [Display(Name = DisplayProfilePictureURL)]
        public string? AuthorProfilePictureURL { get; init; }
    }
}