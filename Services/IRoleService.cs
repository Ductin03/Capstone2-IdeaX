using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Response;

namespace IdeaX.Services
{
    public interface IRoleService
    {
        Task<Responses> CreateRole(CreateRoleRequestModel request);
        Task<List<Role>> GetAllRole();
        Task<Responses> UpdateRole(Guid roleId, UpdateRoleRequestModel model);

        Task<Responses> DeleteRole(Guid roleId);
        Task<Role> GetRoleById(Guid roleId);
    }
}
