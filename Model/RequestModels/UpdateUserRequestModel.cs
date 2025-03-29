using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class UpdateUserRequestModel
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? CCCD { get; set; }
        public string? CCCDBack { get; set; }
        public string? CCCDFront { get; set; }
        public DateTime Birthday { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
    }
}
