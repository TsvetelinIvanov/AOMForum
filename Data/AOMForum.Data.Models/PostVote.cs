using AOMForum.Data.Common.Models;
using AOMForum.Data.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AOMForum.Data.Models
{
    public class PostVote : BaseDeletableModel<int>
    {
        public VoteType Type { get; set; }

        public int PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public virtual Post? Post { get; set; }

        [Required]
        public string? AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public virtual ApplicationUser? Author { get; set; }
    }
}