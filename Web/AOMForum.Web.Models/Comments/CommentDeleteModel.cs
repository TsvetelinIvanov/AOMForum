using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.GlobalConstants;
using static AOMForum.Common.DisplayNames.Comment;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Comments
{
    public class CommentDeleteModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public CommentDeleteModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(IFrameTag);
        }

        public int Id { get; init; }

        [Display(Name = DisplayContent)]
        public string? Content { get; init; }

        [Display(Name = DisplayContent)]
        public string? SanitizedContent => this.sanitizer.Sanitize(this.Content ?? string.Empty);

        public string? AuthorId { get; init; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }

        public string? AuthorProfilePictureURL { get; init; }

        [Display(Name = DisplayVotesCount)]
        public int VotesCount { get; init; }

        [Display(Name = DisplayCreatedOn)]
        public string? CreatedOn { get; init; }        
    }
}