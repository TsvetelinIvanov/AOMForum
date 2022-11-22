using AOMForum.Data.Common.Models;
using AOMForum.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AOMForum.Common.DataConstants.Post;


namespace AOMForum.Data.Models
{
    public class Post : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string? Title { get; set; }

        public PostType Type { get; set; }
        
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string? Content { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public string? AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public virtual ApplicationUser? Author { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        public virtual ICollection<PostTag> Tags { get; set; } = new HashSet<PostTag>();

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<PostReport> Reports { get; set; } = new HashSet<PostReport>();

        public virtual ICollection<PostVote> Votes { get; set; } = new HashSet<PostVote>();
    }
}