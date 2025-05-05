using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Model.ResponseModels;
using IdeaX.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace IdeaX.Services
{
    public class AuthSevice : IAuth
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthSevice(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        //public async Task<string> Authentication(LoginRequestModel request)
        //{
        //    var user = await _unitOfWork.UserRepository.GetByAsync(x => x.Username == request.UserName);
        //    if (user == null)
        //    {
        //        throw new UnauthorizedAccessException("Tài khoản không tồn tại");
        //    }
        //    var roleName = await _unitOfWork.RoleRepository.GetRoleName(request.UserName);
        //    var isAuthen = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        //    if (!isAuthen)
        //    {
        //        throw new UnauthorizedAccessException("Unauthorized");
        //    }
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]!);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //            new Claim(ClaimTypes.Email, user.Email),
        //            new Claim("RoleName", roleName)
        //        }),
        //        Expires = DateTime.UtcNow.AddMinutes(60),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
        //        SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);

        //}
        public async Task<AuthResponseModel> Authentication(LoginRequestModel request)
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

            // Tạo token JWT và refresh token
            return await GenerateTokens(user, roleName);
        }

        // Phương thức xác thực qua Google OAuth
        public async Task<AuthResponseModel> GoogleAuthentication(string googleAccessToken)
        {
            // Gọi API của Google để lấy thông tin người dùng
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", googleAccessToken);

            var response = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var googleUser = JsonSerializer.Deserialize<GoogleUserInfo>(content);

            // Kiểm tra xem người dùng đã tồn tại trong hệ thống chưa
            var user = await _unitOfWork.UserRepository.GetByAsync(x => x.Email == googleUser.Email);

            if (user == null)
            {
                // Tạo người dùng mới nếu chưa tồn tại
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = googleUser.Email,
                    Email = googleUser.Email,
                    FullName = googleUser.Name,
                    Token = "",
                    Phone = "",
                    Address = "",
                    Avatar = "",
                    Birthday = DateTime.UtcNow,
                    CCCD = "",
                    CCCDBack = "",
                    CCCDFront = "",
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    // Đặt mật khẩu ngẫu nhiên vì người dùng sẽ đăng nhập qua Google
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                    IsDeleted = false,
                    // Các trường khác của User
                };

                // Lưu thông tin người dùng vào cơ sở dữ liệu
                await _unitOfWork.UserRepository.CreateAsync(user);

                try
                {
                    await _unitOfWork.SavechangeAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Lỗi khi lưu: " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("👉 Chi tiết: " + ex.InnerException.Message);
                }
                // Gán vai trò mặc định cho người dùng
                await _unitOfWork.RoleRepository.AssignRoleToUser(user.Id, Guid.Parse("f93a1760-18ab-46d1-913d-9c742df7b569"));
              
            }

            // Lấy vai trò của người dùng
            var roleName = await _unitOfWork.RoleRepository.GetRoleName(user.Username);

            // Tạo token JWT và refresh token
            return await GenerateTokens(user, roleName);
        }

        // Phương thức làm mới Access Token bằng Refresh Token
        public async Task<AuthResponseModel> RefreshToken(string refreshToken)
        {
            var user = await _unitOfWork.UserRepository.GetByAsync(x => x.Token == refreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Refresh token không hợp lệ hoặc đã hết hạn");
            }

            var roleName = await _unitOfWork.RoleRepository.GetRoleName(user.Username);

            // Tạo lại token và refresh token mới
            return await GenerateTokens(user, roleName);
        }

        // Phương thức làm mới Access Token với Google Refresh Token
        public async Task<AuthResponseModel> RefreshGoogleToken(string refreshToken)
        {
            var httpClient = _httpClientFactory.CreateClient();

            // Chuẩn bị payload để gửi đến Google OAuth server
            var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", _configuration["Authentication:Google:ClientId"] },
            { "client_secret", _configuration["Authentication:Google:ClientSecret"] },
            { "refresh_token", refreshToken },
            { "grant_type", "refresh_token" }
        });

            // Gửi yêu cầu để làm mới token
            var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", formContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<GoogleTokenResponse>(content);

            // Lấy thông tin người dùng từ Google với access token mới
            return await GoogleAuthentication(tokenResponse.AccessToken);
        }

        // Phương thức tạo JWT access token và refresh token
        private async Task<AuthResponseModel> GenerateTokens(User user, string roleName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.Username ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ?? Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("RoleName", roleName ?? "")
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:AccessTokenExpiryMinutes"])),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessTokenString = tokenHandler.WriteToken(accessToken);

            // Tạo Refresh Token
            var refreshToken = GenerateRefreshToken();
            user.Token = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
                Convert.ToDouble(_configuration["JwtSettings:RefreshTokenExpirationDays"]));

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SavechangeAsync();

            return new AuthResponseModel
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshToken,
                ExpiresIn = Convert.ToInt32(_configuration["JwtSettings:AccessTokenExpiryMinutes"]) * 60
            };
        }

        // Tạo Refresh Token (một chuỗi ngẫu nhiên dài)
        private string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }
    
    }
}
