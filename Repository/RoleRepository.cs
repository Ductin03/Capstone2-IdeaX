using IdeaX.Entities;
using IdeaX.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdeaX.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IdeaXDbContext _context;
        public RoleRepository(IdeaXDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfRoleExist(string roleName)
        {
            var roleExist = await _context.Roles.AnyAsync(x => x.RoleName == roleName);
            return roleExist;
        }

        public async Task<Responses> CreateAsync(Role entity)
        {
            await _context.Roles.AddAsync(entity);
            return new Responses(true, "thêm thành công");
        }

        public Task<Responses> DeleteAsync(Role entity)
        {
            _context.Roles.Update(entity);
            return Task.FromResult(new Responses(true, "Xoa thanh cong"));
        }

        public async Task<Role> FindByIdAsync(Guid id)
        {
            var response = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            return response is not null ? response : null!; 
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();       
        }

        public Task<Role> GetByAsync(Expression<Func<Role, bool>> predicate)
        {
            return _context.Roles.FirstOrDefaultAsync(predicate)!;
        }

        public async Task<string> GetRoleName(string userName)
        {
            var roleName = await (from u in _context.Users
                            join r in _context.Roles on u.RoleId equals r.Id
                            where u.Username == userName
                            select r.RoleName).FirstOrDefaultAsync();
            return roleName!;
        }

        public  Task<Responses> UpdateAsync(Role entity)
        {
            _context.Roles.Update(entity);
            return Task.FromResult(new Responses(true, "Update thanh cong"));
        }
    }
}
