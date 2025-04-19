using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class CreateIdeaRequestModel
    {
        [Required, MaxLength(250)]
        public string Title { get; set; }
        public Guid? CommunityId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Guid InitiatorId { get; set; }
        public string CollaborationType { get; set; }
        public string ImageUrls { get; set; }
        public Guid CategoryId { get; set; }
        public string Status { get; set; }
        public bool CopyrightStatus { get; set; } = false;

        public string? CopyrightCertificate { get; set; }
        public decimal? Price { get; set; }

        public bool? IsForSale { get; set; } = false;
      
        public Guid CreatedBy { get; set; }
        public bool IsPublic { get; set; } = true;
    }
}
