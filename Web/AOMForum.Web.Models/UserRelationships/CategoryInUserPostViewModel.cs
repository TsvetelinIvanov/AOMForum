using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Category;

namespace AOMForum.Web.Models.UserRelationships
{
    public class CategoryInUserPostViewModel
    {
        public int Id { get; init; }

        [Display(Name = DisplayName)]
        public string? Name { get; init; }
    }
}