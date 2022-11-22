using AOMForum.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DataConstants.Setting;

namespace AOMForum.Data.Models
{
    public class Setting : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string? Name { get; set; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string? Content { get; set; }
    }
}