using AOMForum.Data.Common.Models;
using AOMForum.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOMForum.Data.Models
{
    public class CommentVote : BaseModel<int>
    {
        public VoteType Type { get; set; }

        public int CommentId { get; set; }
        [ForeignKey(nameof(CommentId))]
        public virtual Comment? Comment { get; set; }

        public string? AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public virtual ApplicationUser? Author { get; set; }
    }
}