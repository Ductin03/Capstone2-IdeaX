using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class CreateRoleRequestModel
    {
        [Required, MaxLength(255)]
        public string RoleName { get; set; }
        [Required, MaxLength(255)]
        public string Description { get; set; }
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
