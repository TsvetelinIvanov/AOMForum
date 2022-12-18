using System.ComponentModel.DataAnnotations;
using AOMForum.Data.Models.Enums;
using AOMForum.Services.Mapping;
using AOMForum.Data.Models;
using static AOMForum.Common.DisplayNames.Post;
using static AOMForum.Common.DataConstants.Post;
using static AOMForum.Common.DataErrorMessages;

namespace AOMForum.Web.Models.Posts
{
    public class PostInputModel : IMapTo<Post>
    {
        [Display(Name = DisplayTitle)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = StringLengthErrorMessage)]
        public string? Title { get; init; }
        
        //[Required(ErrorMessage = RequiredErrorMessage)]
        //[EnumDataType(typeof(PostType))]
        //[Display(Name = DisplayPostType)]
        //public PostType PostType { get; init; }

        [Display(Name = DisplayContent)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = StringLengthErrorMessage)]
        [DataType(DataType.MultilineText)]
        public string? Content { get; init; }

        [Display(Name = DisplayImageUrl)]
        public string? ImageUrl { get; init; }

        [Required]
        [Display(Name = DisplayCategory)]
        public int CategoryId { get; init; }
                
        [Display(Name = DisplayTagIds)]
        public IEnumerable<int> TagIds { get; set; } = new HashSet<int>();

        public IEnumerable<CategoryInPostViewModel> Categories { get; set; } = new HashSet<CategoryInPostViewModel>();
        //public IEnumerable<CategoryInPostDropDownViewModel> Categories { get; set; } = new HashSet<CategoryInPostDropDownViewModel>();

        public IEnumerable<TagInPostViewModel> Tags { get; set; } = new HashSet<TagInPostViewModel>();
    }
}