using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.GlobalConstants;
using static AOMForum.Common.DisplayNames.Comment;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Comments
{
    public class CommentDetailsViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public CommentDetailsViewModel()
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

        public string? AuthorId { get; init; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }

        [Display(Name = DisplayProfilePictureURL)]
        public string? AuthorProfilePictureURL { get; init; }

        [Display(Name = DisplayVotesCount)]
        public int VotesCount { get; init; }

        public int PostId { get; init; }

        public string? PostAuthorId { get; init; }

        public int? ParentId { get; init; }

        public IEnumerable<CommentDetailsViewModel> Comments { get; set; } = new HashSet<CommentDetailsViewModel>();
    }
}