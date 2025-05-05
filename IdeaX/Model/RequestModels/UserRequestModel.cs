using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class UserRequestModel
    {
        [Required, MaxLength(250)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(250)]
        public string Username { get; set; }
        [Required, EmailAddress, MaxLength(250)]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
        [Required]
        [MaxLength(250)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [MaxLength(250)]
        public string CCCD { get; set; }
        [Required]
        public string CCCDBack { get; set; }
        [Required]
        public string CCCDFront { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength(50)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(250)]
        public string Address { get; set; }
     
        public DateTime CreatedOn { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
