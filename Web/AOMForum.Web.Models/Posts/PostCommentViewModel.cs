using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using AOMForum.Data.Models;
using AOMForum.Services.Mapping;
using static AOMForum.Common.DisplayNames.Comment;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Posts
{
    public class PostCommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; init; }

        public int ParentId { get; init; }

        [Display(Name = DisplayCreatedOn)]
        public DateTime CreatedOn { get; init; }

        [Display(Name = DisplayContent)]
        public string? Content { get; init; }

        [Display(Name = DisplayContent)]
        public string? SanitizedContent => new HtmlSanitizer().Sanitize(this.Content ?? string.Empty);

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }
    }
}