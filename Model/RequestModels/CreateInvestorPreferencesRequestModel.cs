using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class CreateInvestorPreferencesRequestModel
    {
        [Required, MaxLength(250)]
        public string PreferredIndustries { get; set; }
        [Required, MaxLength(250)]
        public string PreferredStages { get; set; }
        [Required, MaxLength(250)]
        public string PreferredRegions { get; set; }
        [Required]
        public decimal FundingRangeMin { get; set; }
        [Required]
        public decimal FundingRangeMax { get; set; }
    }
}
