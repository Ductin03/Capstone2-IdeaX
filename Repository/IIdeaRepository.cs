using IdeaX.Entities;
using IdeaX.interfaces;
using IdeaX.Model.RequestModels;
using IdeaX.Model.ResponseModels;

namespace IdeaX.Repository
{
    public interface IIdeaRepository : IGenericInterface<Idea>
    {
        Task<BasePaginationResponseModel<GetIdeaResponseModel>> GetAllIdeasPublicAsync(GetIdeaRequestModel model);
        Task<BasePaginationResponseModel<GetIdeaResponseModel>> GetAllIdeaPrivateForInvestorAsync(GetIdeaRequestModel model);

        Task<List<GetIdeaResponseModel>> GetIdeaByUserIdAsync(Guid userId);
        Task<List<GetIdeaResponseModel>> GetIdeaByCategoryAsync(Guid categoryId);
    }
}
