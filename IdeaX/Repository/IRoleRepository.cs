using IdeaX.Entities;
using IdeaX.interfaces;

namespace IdeaX.Repository
{
    public interface IRoleRepository : IGenericInterface<Role>
    {    
        Task<bool> CheckIfRoleExist (string roleName);
        Task<string> GetRoleName (string userName);  
        Task AssignRoleToUser(Guid userId, Guid roleId);
    }
}
