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

        [Display(Name = DisplayCreatedOn)]
        public string? CreatedOn { get; init; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }

        [Display(Name = DisplayProfilePictureURL)]
        public string? AuthorProfilePictureURL { get; init; }

        public int PostId { get; init; }

        [Display(Name = DisplayPostTitle)]
        public string? PostTitle { get; init; }        

        [Display(Name = DisplayChildrenCount)]
        public int ChildrenCount { get; init; }
    }
}