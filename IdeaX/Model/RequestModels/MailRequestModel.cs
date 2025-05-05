using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class MailRequestModel
    {
        [Required]
        [MaxLength(250)]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        public string Subject { get; set; }
        [Required]
        [MaxLength(250)]
        public string EmailBody { get; set; }
    }
}
