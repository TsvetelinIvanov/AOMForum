using AOMForum.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AOMForum.Common.DataConstants.Message;

namespace AOMForum.Data.Models
{
    public class Message : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string? Content { get; set; }

        [Required]
        public string? SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public virtual ApplicationUser? Sender { get; set; }

        [Required]
        public string? ReceiverId { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public virtual ApplicationUser? Receiver { get; set; }
    }
}