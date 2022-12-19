using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.GlobalConstants;
using static AOMForum.Common.DisplayNames.Post;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Posts
{
    public class PostDetailsViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public PostDetailsViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(IFrameTag);
        }

        public int Id { get; init; }

        [Display(Name = DisplayTitle)]
        public string? Title { get; init; }

        [Display(Name = DisplayCreatedOn)]
        public string? CreatedOn { get; init; }

        [Display(Name = DisplayContent)]
        public string? Content { get; init; }

        [Display(Name = DisplayContent)]
        public string? SanitizedContent => this.sanitizer.Sanitize(this.Content ?? string.Empty);

        [Display(Name = DisplayImageUrl)]
        public string? ImageUrl { get; init; }

        [Display(Name = DisplayCommentsCount)]
        public int CommentsCount { get; init; }      

        [Display(Name = DisplayVotesCount)]
        public int VotesCount { get; init; }

        public string? AuthorId { get; init; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }

        [Display(Name = DisplayProfilePictureURL)]
        public string? AuthorProfilePictureURL { get; init; }

        [Display(Name = DisplayCategory)]
        public CategoryInPostViewModel? Category { get; init; }

        public IEnumerable<TagInPostViewModel> Tags { get; set; } = new HashSet<TagInPostViewModel>();

        public IEnumerable<CommentInPostViewModel> Comments { get; set; } = new HashSet<CommentInPostViewModel>();
    }
}