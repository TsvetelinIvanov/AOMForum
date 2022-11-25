using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.PostReport;
using static AOMForum.Common.DataConstants.PostReport;
using static AOMForum.Common.DataErrorMessages;

namespace AOMForum.Web.Models.PostReports
{
    public class PostReportEditModel
    {
        public int Id { get; init; }

        public int PostId { get; init; }

        [Display(Name = DisplayContent)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = StringLengthErrorMessage)]
        public string? Content { get; init; }
    }
}