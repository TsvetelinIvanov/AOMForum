using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Tag;

namespace AOMForum.Web.Models.Posts
{
    public class TagInPostViewModel
    {
        public int Id { get; init; }

        [Display(Name = DisplayName)]
        public string? Name { get; init; }
    }
}