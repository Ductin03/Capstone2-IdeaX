using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class UpdateRoleRequestModel
    {
        [MaxLength(255)]
        public string? RoleName { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
