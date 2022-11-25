using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Category;
using static AOMForum.Common.DataConstants.Category;
using static AOMForum.Common.DataErrorMessages;

namespace AOMForum.Web.Models.Categories
{
    public class CategoryInputModel
    {
        [Display(Name = DisplayName)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = StringLengthErrorMessage)]        
        public string? Name { get; init; }

        [Display(Name = DisplayDescription)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = StringLengthErrorMessage)]
        public string? Description { get; init; }

        public string? ImageURL { get; init; }
    }
}