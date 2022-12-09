using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Message;
using static AOMForum.Common.DataConstants.Message;
using static AOMForum.Common.DataErrorMessages;

namespace AOMForum.Web.Models.Messages
{
    public class MessageInputModel
    {
        [Display(Name = DisplayContent)]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = StringLengthErrorMessage)]
        public string? Content { get; set; }

        [Required]
        public string? ReceiverId { get; set; }

        public IEnumerable<MessagePartnerModel> MessagePartners { get; set; } = new HashSet<MessagePartnerModel>();
    }
}