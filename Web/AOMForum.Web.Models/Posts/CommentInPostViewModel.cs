using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.GlobalConstants;
using static AOMForum.Common.DisplayNames.Comment;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Posts
{
    public class CommentInPostViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public CommentInPostViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(IFrameTag);
        }

        public int Id { get; init; }

        [Display(Name = DisplayContent)]
        public string? Content { get; init; }

        [Display(Name = DisplayContent)]
        public string? SanitizedContent => this.sanitizer.Sanitize(this.Content ?? string.Empty);

        [Display(Name = DisplayVotesCount)]
        public int VotesCount { get; set; }

        [Display(Name = DisplayCreatedOn)]
        public string? CreatedOn { get; init; }

        public int? ParentId { get; init; }

        public string? AuthorId { get; init; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }

        public string? AuthorProfilePictureURL { get; init; }
    }
}