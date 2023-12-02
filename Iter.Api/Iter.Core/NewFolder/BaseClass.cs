using System.ComponentModel.DataAnnotations;

namespace Iter.Core.Models
{
    public class BaseClass : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
