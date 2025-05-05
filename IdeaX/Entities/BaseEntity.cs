using System.ComponentModel.DataAnnotations;

namespace IdeaX.Entities
{
    public abstract class BaseEntity
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
