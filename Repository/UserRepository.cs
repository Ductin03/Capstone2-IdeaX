using IdeaX.Entities;
using IdeaX.interfaces;
using IdeaX.Model.ResponseModels;
using IdeaX.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdeaX.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IdeaXDbContext _context;
        public UserRepository(IdeaXDbContext context)
        {
            _context = context;
        }

        public async Task<User> CheckIfEmailExistAsync(string email)
        {
            var userExist = await _context.Users.FirstOrDefaultAsync( x => x.Email == email);
            return userExist;
        }

        public async Task<bool> CheckIfUserExistAsync(string username)
        {
            var emailExist = await _context.Users.AnyAsync(x => x.Username == username);
            return emailExist;
        }

        public async Task<Responses> CreateAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            return new Responses(true, "Đăng ký thành công");
        }

        public Task<Responses> DeleteAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Verification> FindEmailByOtp(string otp)
        {
            var user = await _context.Verifications.FirstOrDefaultAsync(x => x.OTP == otp);
            return user;
        }

        public Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task<UserResponseModel> GetInfoUserByIdAsync(Guid id)
        {
            return await (from u in _context.Users
                         join r in _context.Roles on u.RoleId equals r.Id
                         where u.Id == id
                         select new UserResponseModel
                         {
                             Id = u.Id,
                             Username = u.Username,
                             FullName = u.FullName, 
                             RoleName = r.RoleName,
                             Phone = u.Phone,
                             Email = u.Email,
                             Address = u.Address,
                             Avatar = u.Avatar,
                             CCCD = u.CCCD,
                             CCCDBack = u.CCCDBack,
                             CCCDFront = u.CCCDFront,
                         }).FirstOrDefaultAsync();
        }

        public async Task<Responses> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            return new Responses(true, "Update thanh cong");
        }
    }
}
