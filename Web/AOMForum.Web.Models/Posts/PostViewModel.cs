using AutoMapper;
using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using AOMForum.Data.Models;
using AOMForum.Services.Mapping;
using static AOMForum.Common.DisplayNames.Post;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Posts
{
    public class PostViewModel : IMapFrom<Post>, IMapTo<Post>, IHaveCustomMappings
    {
        public int Id { get; init; }

        [Display(Name = DisplayTitle)]
        public string? Title { get; init; }

        [Display(Name = DisplayContent)]
        public string? Content { get; init; }

        [Display(Name = DisplayContent)]
        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content ?? string.Empty);

        [Display(Name = DisplayCreatedOn)]
        public DateTime CreatedOn { get; init; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }

        [Display(Name = DisplayVotesCount)]
        public int VotesCount { get; init; }

        public IEnumerable<PostCommentViewModel> Comments { get; set; } = new HashSet<PostCommentViewModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
                .ForMember(x => x.VotesCount, options =>
                {
                    options.MapFrom(p => p.Votes.Sum(v => (int)v.Type));
                });
        }
    }
}