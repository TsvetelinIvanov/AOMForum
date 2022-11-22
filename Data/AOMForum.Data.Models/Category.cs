using AOMForum.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DataConstants.Category;

namespace AOMForum.Data.Models
{
    public class Category : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string? Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}