using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Message;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Messages
{
    public class LastMessageModel
    {
        public string? Id { get; init; }

        [Display(Name = DisplayLastMessage)]
        public string? LastMessage { get; init; }

        [Display(Name = DisplayLastMessageActivity)]
        public string? LastMessageActivity { get; init; }

        [Display(Name = DisplayUserName)]
        public string? UserName { get; init; }

        [Display(Name = DisplayProfilePictureURL)]
        public string? ProfilePictureURL { get; init; }
    }
}