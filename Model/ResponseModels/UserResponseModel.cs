using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.ResponseModels
{
    public class UserResponseModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string CCCD { get; set; }
        public string CCCDFront { get; set; } // Ảnh CCCD mặt trước 
        public string CCCDBack { get; set; } // Ảnh CCCD mặt sau 
        public DateTime CreatedOn { get; set; }
    }
}
