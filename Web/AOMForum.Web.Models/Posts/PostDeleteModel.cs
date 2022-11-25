﻿using Ganss.Xss;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.GlobalConstants;
using static AOMForum.Common.DisplayNames.Post;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.Posts
{
    public class PostDeleteModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public PostDeleteModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(IFrameTag);
        }

        public int Id { get; init; }

        [Display(Name = DisplayTitle)]
        public string? Title { get; init; }

        [Display(Name = DisplayCreatedOn)]
        public string? CreatedOn { get; init; }

        [Display(Name = DisplayContent)]
        public string? Content { get; init; }

        [Display(Name = DisplayContent)]
        public string? SanitizedContent => this.sanitizer.Sanitize(this.Content ?? string.Empty);

        [Display(Name = DisplayCommentsCount)]
        public int CommentsCount { get; init; }

        public int Views { get; init; }

        [Display(Name = DisplayVotesCount)]
        public int VotesCount { get; init; }

        public string? AuthorId { get; init; }

        [Display(Name = DisplayUserName)]
        public string? AuthorUserName { get; init; }

        public string? AuthorProfilePictureURL { get; init; }

        public CategoryInPostViewModel? Category { get; init; }

        public IEnumerable<TagInPostViewModel> Tags { get; set; } = new HashSet<TagInPostViewModel>();
    }
}