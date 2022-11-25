using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.CommentReport;
using static AOMForum.Common.DataConstants.CommentReport;
using static AOMForum.Common.DataErrorMessages;

namespace AOMForum.Web.Models.CommentReports
{
    public class CommentReportEditModel
    {
        public int Id { get; init; }

        public int CommentId { get; init; }

        [Display(Name = DisplayContent)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = StringLengthErrorMessage)]
        public string? Content { get; init; }
    }
}