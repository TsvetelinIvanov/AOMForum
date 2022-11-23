using AOMForum.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOMForum.Data.Models
{
    public class Relationship : BaseDeletableModel<int>
    {
        public string? LeaderId { get; set; }
        public virtual ApplicationUser? Leader { get; set; }
        
        public string? FollowerId { get; set; }
        public virtual ApplicationUser? Follower { get; set; }
    }
}