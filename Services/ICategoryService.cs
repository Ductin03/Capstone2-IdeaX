using IdeaX.Model.RequestModels;
using IdeaX.Response;

namespace IdeaX.Services
{
    public interface ICategoryService
    {
        Task<Responses> UpdateCategory(UpdateCategoryRequestModel request, Guid categoryId);
        Task<Responses> RemoveCategory(Guid categoryId);
    }
}
