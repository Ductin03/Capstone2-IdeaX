using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Model.ResponseModels;
using IdeaX.Response;

namespace IdeaX.Services
{
    public interface IIdeaService
    {
        Task<Responses> CreateIdea(CreateIdeaRequestModel request);
        Task<Responses> UpdateIdea(UpdateIdeaRequestModel request, Guid id);
        Task<Responses> DeleteIdea(Guid id);
        Task<BasePaginationResponseModel<GetIdeaResponseModel>> GetAllIdeasPublic(GetIdeaRequestModel request);
        Task<BasePaginationResponseModel<GetIdeaResponseModel>> GetAllIdeasPrivateForInvestor(GetIdeaRequestModel request);

        Task<List<GetIdeaResponseModel>> GetIdeaByUserId(Guid userId);
        Task<List<GetIdeaResponseModel>> GetIdeaByCategory(Guid categoryId);
    }
}
