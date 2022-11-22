using AOMForum.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DataConstants.Tag;

namespace AOMForum.Data.Models
{
    public class Tag : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string? Name { get; set; }        

        public virtual ICollection<PostTag> Posts { get; set; } = new HashSet<PostTag>();
    }
}