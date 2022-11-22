using AOMForum.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AOMForum.Common.DataConstants.Comment;

namespace AOMForum.Data.Models
{
    public class Comment : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string? Content { get; set; }        

        public int? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public Comment? Parent { get; set; }

        [Required]
        public int PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public virtual Post? Post { get; set; }

        [Required]
        public string? AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public virtual ApplicationUser? Author { get; set; }
        
        public virtual ICollection<CommentReport> Reports { get; set; } = new HashSet<CommentReport>();

        public virtual ICollection<CommentVote> Votes { get; set; } = new HashSet<CommentVote>();
    }
}