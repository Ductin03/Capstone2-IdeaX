using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdeaX.Entities
{
    public class Idea : BaseEntity
    {
        [Required,MaxLength(250)]
        public string IdeaCode { get; set; }

        [Required, MaxLength(250)]
        public string Title { get; set; }
        [Required, MaxLength(250)]
        public string CollaborationType { get; set; }
        [Required, MaxLength(250)]
        public string ImageUrls { get; set; }
        public  Guid? CommunityId { get; set; }

        [Required, MaxLength(250)]
        public string Description { get; set; }
        [Required]
        public Guid InitiatorId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        public string Status { get; set; }
        [Required]
        public bool CopyrightStatus { get; set; } = false;
        [Required]

        public string? CopyrightCertificate { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public bool? IsForSale { get; set; } = false;
        public Guid? InvestorId { get; set; } // ID nhà đầu tư (nếu có)
        public DateTime? InvestmentDate { get; set; } // Ngày đầu tư
        public int TotalViews { get; set; } = 0;
        public int TotalLikes { get; set; } = 0;
        public int TotalComments { get; set; } = 0;
        public int TotalRatings { get; set; } = 0;
        public bool isPublic { get; set; }
    }
}

