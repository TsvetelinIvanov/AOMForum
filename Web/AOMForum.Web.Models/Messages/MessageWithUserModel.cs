using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Message;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Messages
{
    public class MessageWithUserModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public MessageWithUserModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        [Display(Name = DisplayContent)]
        public string? Content { get; init; }

        [Display(Name = DisplayContent)]
        public string? SanitizedContent => this.sanitizer.Sanitize(this.Content ?? string.Empty);

        public string? AuthorId { get; init; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }

        public string? AuthorProfilePicture { get; init; }

        [Display(Name = DisplayCreatedOn)]
        public string? CreatedOn { get; init; }
    }
}