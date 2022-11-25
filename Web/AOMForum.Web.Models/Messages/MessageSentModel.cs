using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Message;
using static AOMForum.Common.DataConstants.Message;
using static AOMForum.Common.DataErrorMessages;

namespace AOMForum.Web.Models.Messages
{
    public class MessageSentModel
    {
        [Display(Name = DisplayContent)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = StringLengthErrorMessage)]
        public string? Content { get; init; }

        [Required]
        public string? ReceiverId { get; init; }

        public IEnumerable<MessageUserModel> Users { get; set; } = new HashSet<MessageUserModel>();
    }
}