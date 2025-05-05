using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class RefreshGoogleTokenRequest
    {
        [Required]
        public string GoogleRefreshToken { get; set; }
    }
}
