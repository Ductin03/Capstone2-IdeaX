using System.ComponentModel.DataAnnotations;

namespace IdeaX.Entities
{
    public class Role : BaseEntity
    {
        [Required, MaxLength(255)]
        public string RoleName { get; set; }
        [Required, MaxLength(255)]
        public string Description{ get; set; }
    }
}
