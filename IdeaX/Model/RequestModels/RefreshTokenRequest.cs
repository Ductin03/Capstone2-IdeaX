using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class RefreshTokenRequest
    {

        [Required]
        public string RefreshToken { get; set; }
    }
}
