using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.ResponseModels
{
    public class GetIdeaResponseModel
    {
        public Guid Id { get; set; }
        public string IdeaCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Initiator { get; set; }
        public string Category{ get; set; }
        public  string ImageUrls { get; set; }
        public string Status { get; set; }
        public bool CopyrightStatus { get; set; } = false;
        public string? CopyrightCertificate { get; set; }
        public decimal? Price { get; set; }
        public bool? IsForSale { get; set; } = false;
        public string Investor { get; set; } // ID nhà đầu tư (nếu có)
        public DateTime? InvestmentDate { get; set; } // Ngày đầu tư
        public int TotalViews { get; set; } = 0;
        public int TotalLikes { get; set; } = 0;
        public int TotalComments { get; set; } = 0;
        public int TotalRatings { get; set; } = 0;
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
