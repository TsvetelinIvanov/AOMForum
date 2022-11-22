using AOMForum.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOMForum.Data.Models
{
    public class Relationship : BaseDeletableModel<int>
    {
        [Required]
        public string? LeaderId { get; set; }
        [ForeignKey(nameof(LeaderId))]
        public virtual ApplicationUser? Leader { get; set; }

        [Required]
        public string? FollowerId { get; set; }
        [ForeignKey(nameof(LeaderId))]
        public virtual ApplicationUser? Follower { get; set; }
    }
}