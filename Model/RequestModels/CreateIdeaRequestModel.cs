using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class CreateIdeaRequestModel
    {
        [Required, StringLength(255)]
        public string Title { get; set; }
        public Guid? CommunityId { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public Guid InitiatorId { get; set; }

        [Required, StringLength(100)]
        public Guid CategoryId { get; set; }
        public string Status { get; set; }
        public bool CopyrightStatus { get; set; } = false;

        public string? CopyrightCertificate { get; set; }
        public decimal? Price { get; set; }

        public bool? IsForSale { get; set; } = false;
        public Guid? InvestorId { get; set; } // ID nhà đầu tư (nếu có)
        public DateTime? InvestmentDate { get; set; } // Ngày đầu tư
        public int? TotalViews { get; set; } = 0;
        public int? TotalLikes { get; set; } = 0;
        public int? TotalComments { get; set; } = 0;
        public int? TotalRatings { get; set; } = 0;
        public Guid CreatedBy { get; set; }
    }
}
