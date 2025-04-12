using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Response;

namespace IdeaX.Services
{
    public interface IIdeaService
    {
        Task<Responses> CreateIdea(CreateIdeaRequestModel request);
        Task<Responses> UpdateIdea(UpdateIdeaRequestModel request, Guid id);
        Task<Responses> DeleteIdea(Guid id);
        Task<List<Idea>> GetAllIdea();
    }
}
