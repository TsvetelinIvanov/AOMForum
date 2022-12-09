using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Message;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Messages
{
    public class PartnerMessageModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public PartnerMessageModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        [Display(Name = DisplayContent)]
        public string? Content { get; init; }

        [Display(Name = DisplayContent)]
        public string? SanitizedContent => this.sanitizer.Sanitize(this.Content ?? string.Empty);

        [Display(Name = DisplayCreatedOn)]
        public string? CreatedOn { get; init; }

        public string? SenderId { get; init; }

        [Display(Name = DisplayUserName)]
        public string? SenderUserName { get; init; }

        [Display(Name = DisplayProfilePictureURL)]
        public string? SenderProfilePictureURL { get; init; }
    }
}