using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Tag;
using static AOMForum.Common.DataConstants.Tag;
using static AOMForum.Common.DataErrorMessages;

namespace AOMForum.Web.Models.Tags
{
    public class TagEditModel
    {
        [Display(Name = DisplayName)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = StringLengthErrorMessage)]
        public string? Name { get; init; }
    }
}