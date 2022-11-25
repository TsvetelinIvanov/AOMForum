using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Post;
using static AOMForum.Common.DataConstants.Post;
using static AOMForum.Common.DataErrorMessages;

namespace AOMForum.Web.Models.Posts
{
    public class PostEditModel
    {
        public int Id { get; init; }

        [Display(Name = DisplayTitle)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = StringLengthErrorMessage)]
        public string? Title { get; init; }

        [Display(Name = DisplayContent)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = StringLengthErrorMessage)]
        [DataType(DataType.MultilineText)]
        public string? Content { get; init; }

        [Required]        
        public int CategoryId { get; init; }

        public string? AuthorId { get; init; }

        [Display(Name = DisplayTagIds)]
        public IEnumerable<int> TagIds { get; init; } = new HashSet<int>();

        public IEnumerable<CategoryInPostViewModel> Categories { get; set; } = new HashSet<CategoryInPostViewModel>();
        //public IEnumerable<CategoryInPostDropDownViewModel> Categories { get; set; } = new HashSet<CategoryInPostDropDownViewModel>();

        public IEnumerable<TagInPostViewModel> Tags { get; set; } = new HashSet<TagInPostViewModel>();
    }
}