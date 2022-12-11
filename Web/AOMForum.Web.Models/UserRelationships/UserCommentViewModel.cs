using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Comment;

namespace AOMForum.Web.Models.UserRelationships
{
    public class UserCommentViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public UserCommentViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        public int Id { get; init; }

        public int PostId { get; init; }

        [Display(Name = DisplayPostTitle)]
        public string? PostTitle { get; init; }

        [Display(Name = DisplayContent)]
        public string? Content { get; init; }

        [Display(Name = DisplayContent)]
        public string? SanitizedContent => this.sanitizer.Sanitize(this.Content ?? string.Empty);

        public int PostCategoryId { get; init; }

        [Display(Name = DisplayPostCategoryName)]
        public string? PostCategoryName { get; init; }

        [Display(Name = DisplayVotesCount)]
        public int VotesCount { get; init; }
    }
}