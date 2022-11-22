using AOMForum.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AOMForum.Common.DataConstants.CommentReport;

namespace AOMForum.Data.Models
{
    public class CommentReport : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string? Content { get; set; }

        [Required]
        public int CommentId { get; set; }
        [ForeignKey(nameof(CommentId))]
        public virtual Comment? Comment { get; set; }

        [Required]
        public string? AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public virtual ApplicationUser? Author { get; set; }
    }
}