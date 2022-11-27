using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Post;

namespace AOMForum.Web.Models.Users
{
    public class UserRelationshipViewModel
    {
        public int Id { get; init; }

        [Display(Name = DisplayTitle)]
        public string? Title { get; init; }

        [Display(Name = DisplayVotesCount)]
        public int VotesCount { get; init; }

        [Display(Name = DisplayCommentsCount)]
        public int CommentsCount { get; init; }

        public int Views { get; init; }

        public string? Activity { get; init; }        

        public CategoryInUseRelationshipViewModel? Category { get; set; }

        public IEnumerable<TagInUserRelationshipViewModel> Tags { get; set; } = new HashSet<TagInUserRelationshipViewModel>();
    }
}