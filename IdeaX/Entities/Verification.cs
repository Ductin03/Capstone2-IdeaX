using System.ComponentModel.DataAnnotations;

namespace IdeaX.Entities
{
    public class Verification : BaseEntity
    {
        [Required, EmailAddress, MaxLength(250)]
        public string Email { get; set; } = string.Empty;
        [Required, MaxLength(250)]
        public string OTP { get; set; } = string.Empty;
        public DateTime ExpirationTime { get; set; } = DateTime.UtcNow.AddMinutes(3); // OTP hết hạn sau 3 phút
        public string Data { get; set; } = string.Empty; // Lưu thông tin người dùng dạng JSON
    }
}
