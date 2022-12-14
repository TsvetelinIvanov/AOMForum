using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Comment;
using static AOMForum.Common.DataConstants.Comment;
using static AOMForum.Common.DataErrorMessages;

namespace AOMForum.Web.Models.Comments
{
    public class CommentInputModel
    {
        public int PostId { get; init; }

        public int? ParentId { get; init; }

        [Display(Name = DisplayContent)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = StringLengthErrorMessage)]
        public string? Content { get; init; }
    }
}