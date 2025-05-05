using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Response;
using IdeaX.UnitOfWork;

namespace IdeaX.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Responses> CreateRole(CreateRoleRequestModel request)
        {
            var roleExist = await _unitOfWork.RoleRepository.CheckIfRoleExist(request.RoleName);
            if (roleExist)
            {
                return new Responses(false, "Role đã tồn tại");
            }
            var role = new Role
            {
                Id = Guid.NewGuid(),
                RoleName = request.RoleName,
                Description = request.Description,
                CreatedBy = request.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false            
            };
            await _unitOfWork.RoleRepository.CreateAsync(role);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, "Thêm Role thành công");
            
        }

        public async Task<Responses> DeleteRole(Guid roleId)
        {
            var roleExist = await _unitOfWork.RoleRepository.FindByIdAsync(roleId);
            if(roleExist == null)
            {
                return new Responses(false, "Khong ton tai roleId");
            }
            roleExist.IsDeleted = true;
            await _unitOfWork.RoleRepository.UpdateAsync(roleExist);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, $"xoa thanh cong quyen {roleExist.RoleName}");
        }

        public async Task<List<Role>> GetAllRole()
        {
            var response = await _unitOfWork.RoleRepository.GetAllAsync();
            return response;
        }

        public async Task<Role> GetRoleById(Guid roleId)
        {
            return await _unitOfWork.RoleRepository.GetByAsync(r => r.Id == roleId);
        }

        public async Task<Responses> UpdateRole(Guid roleId, UpdateRoleRequestModel model)
        {
            var roleExist = await _unitOfWork.RoleRepository.FindByIdAsync(roleId);
            if(roleExist == null)
            {
                return new Responses(false, "role khong ton tai");
            }
            roleExist.RoleName = model.RoleName;
            roleExist.Description = model.Description;
            roleExist.UpdatedBy = model.UpdatedBy;
            roleExist.UpdatedOn = DateTime.UtcNow;
            await _unitOfWork.RoleRepository.UpdateAsync(roleExist);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, "Cap nhat thanh cong");
        }
    }
}
