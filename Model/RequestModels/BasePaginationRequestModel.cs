using System.ComponentModel.DataAnnotations;

namespace IdeaX.Model.RequestModels
{
    public class BasePaginationRequestModel
    {
        [MaxLength(250)]
        public string? Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
