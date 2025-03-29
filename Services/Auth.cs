using IdeaX.Model.RequestModels;
using IdeaX.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdeaX.Services
{
    public class Auth : IAuth
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public Auth(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<string> Authentication(LoginRequestModel request)
        {
            var user = await _unitOfWork.UserRepository.GetByAsync(x => x.Username == request.UserName);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Tài khoản không tồn tại");
            }
            var roleName = await _unitOfWork.RoleRepository.GetRoleName(request.UserName);
            var isAuthen = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isAuthen)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("RoleName", roleName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
