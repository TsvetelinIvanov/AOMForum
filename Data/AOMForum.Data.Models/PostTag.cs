using AOMForum.Data.Common.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOMForum.Data.Models
{
    public class PostTag : BaseDeletableModel<int>
    {
        public int PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public virtual Post? Post { get; set; }

        public int TagId { get; set; }
        [ForeignKey(nameof(TagId))]
        public virtual Tag? Tag { get; set; }
    }
}