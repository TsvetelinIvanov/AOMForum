using System.ComponentModel.DataAnnotations;

namespace AOMForum.Data.Common.Models
{
    public class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeletedOn { get; set; }
    }
}