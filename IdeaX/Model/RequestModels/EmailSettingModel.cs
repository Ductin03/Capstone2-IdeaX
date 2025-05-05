using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class EmailSettingModel
    {
        [Required]
        [MaxLength(250)]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
        [Required]
        [MaxLength(250)]
        public string Host { get; set; }
        [Required]
        [MaxLength(250)]
        public string DisplayName { get; set; }
        [Required]
        [MaxLength(250)]
        public int Port { get; set; }
    }
}
