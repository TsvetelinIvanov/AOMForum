using System.Net;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using AOMForum.Data.Models;
using AOMForum.Services.Mapping;
using static AOMForum.Common.DataConstants.Post;
using static AOMForum.Common.DisplayNames.Post;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Categories
{
    public class PostInCategoryViewModel : IMapFrom<Post>
    {
        public int Id { get; init; }

        [Display(Name = DisplayCreatedOn)]
        public DateTime CreatedOn { get; init; }

        [Display(Name = DisplayTitle)]
        public string? Title { get; init; }

        public string? Content { get; init; }

        [Display(Name = DisplayShortContent)]
        public string ShortContent
        {
            get
            {
                string content = WebUtility.HtmlDecode(Regex.Replace(this.Content ?? string.Empty, @"<[^>]+>", string.Empty));

                return content.Length > ShortContentMaxLength ? content.Substring(0, ShortContentMaxLength) + "..." : content;
            }
        }

        [Display(Name = DisplayUserName)]
        public string? UserUserName { get; init; }

        [Display(Name = DisplayCommentsCount)]
        public int CommentsCount { get; init; }
    }
}