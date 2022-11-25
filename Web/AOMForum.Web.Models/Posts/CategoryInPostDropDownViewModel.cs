using AOMForum.Data.Models;
using AOMForum.Services.Mapping;
using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.Category;

namespace AOMForum.Web.Models.Posts
{
    public class CategoryInPostDropDownViewModel : IMapFrom<Category>
    {
        public int Id { get; init; }

        [Display(Name = DisplayName)]        
        public string? Name { get; init; }
    }
}