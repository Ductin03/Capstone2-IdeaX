using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdeaX.Entities
{
    public class Idea
    {
        public enum IdeaStatus
        {
            DangTimDauTu,
            ChiDeThamKhao,
            HopTacDauTu
        }

        public enum IdeaType
        {
            ForSale,
            ForReference,
            SeekingInvestment
        }
        [Required, StringLength(20)]
        public string IdeaCode { get; set; } = $"IDEA-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

        [Required, StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, StringLength(100)]
        public Guid CategoryId { get; set; }


        public IdeaStatus Status { get; set; }

        public IdeaType Type { get; set; }

        public bool CopyrightStatus { get; set; } = false;

        public string? CopyrightCertificate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public bool? IsForSale { get; set; } = false;
        public long? InvestorId { get; set; } // ID nhà đầu tư (nếu có)
        public DateTime? InvestmentDate { get; set; } // Ngày đầu tư
        public int TotalViews { get; set; } = 0;
        public int TotalLikes { get; set; } = 0;
        public int TotalShares { get; set; } = 0;
        public int TotalComments { get; set; } = 0;
        public int TotalRatings { get; set; } = 0;

        [Column(TypeName = "decimal(3,2)")]
        public decimal? AverageRating { get; set; }
    }
}

