using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Post;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Posts
{
    public class PostListViewModel
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
                
        public string? AuthorId { get; init; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }

        public string? AuthorProfilePicture { get; init; }

        public CategoryInPostViewModel? Category { get; init; }

        public IEnumerable<TagInPostViewModel> Tags { get; set; } = new HashSet<TagInPostViewModel>();
    }
}