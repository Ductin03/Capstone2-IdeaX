using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace IdeaX.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        public string Username { get; set; }
        [Required, EmailAddress, MaxLength(250)]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        public string Token { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        [Required, MaxLength(250)]
        public string FullName{ get; set; }
        [Required]
        [MaxLength(250)]
        public string PasswordHash { get; set; }
        public Guid RoleId { get; set; }
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength(50)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(250)]
        public string Avatar { get; set; }
        [Required]
        [MaxLength(250)]
        public string Address { get; set; }
        [Required]
        [MaxLength(250)]
        public string CCCD { get; set; }
        [Required]
        [MaxLength(250)]
        public string CCCDFront { get; set; } // Ảnh CCCD mặt trước 

        [Required]
        [MaxLength(250)]
        public string CCCDBack { get; set; } // Ảnh CCCD mặt sau 
        public string? PreferredIndustries { get; set; }
        public string? PreferredStages { get; set; }
        public string? PreferredRegions { get; set; }
        public decimal? FundingRangeMin { get; set; }
        public decimal? FundingRangeMax { get; set; }

    }
}
