using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Comment;
using static AOMForum.Common.DataConstants.Comment;
using static AOMForum.Common.DataErrorMessages;

namespace AOMForum.Web.Models.Comments
{
    public class CommentEditModel
    {
        public int Id { get; init; }

        public int AuthorId { get; init; }

        [Display(Name = DisplayContent)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = StringLengthErrorMessage)]
        public string? Content { get; init; }
    }
}