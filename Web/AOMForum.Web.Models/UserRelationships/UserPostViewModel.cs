using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Post;

namespace AOMForum.Web.Models.UserRelationships
{
    public class UserPostViewModel
    {
        public int Id { get; init; }

        [Display(Name = DisplayTitle)]
        public string? Title { get; init; }

        [Display(Name = DisplayVotesCount)]
        public int VotesCount { get; init; }

        [Display(Name = DisplayCommentsCount)]
        public int CommentsCount { get; init; }

        public CategoryInUserPostViewModel? Category { get; set; }

        public IEnumerable<TagInUserPostViewModel> Tags { get; set; } = new HashSet<TagInUserPostViewModel>();
    }
}