using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class UpdateIdeaRequestModel
    {
        [Required,MaxLength(250)]
        public string Title { get; set; }
        public Guid? CommunityId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        public string Status { get; set; }
        public bool CopyrightStatus { get; set; } = false;

        public string? CopyrightCertificate { get; set; }
        public decimal? Price { get; set; }

        public bool? IsForSale { get; set; } = false;
    }
}
