using IdeaX.Model.RequestModels;
using IdeaX.Model.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace IdeaX.Services
{
    public interface IAuth
    {
        Task<AuthResponseModel> Authentication(LoginRequestModel request);

        // Xác thực với Google OAuth
        Task<AuthResponseModel> GoogleAuthentication(string googleAccessToken);

        // Làm mới Access Token với Refresh Token nội bộ
        Task<AuthResponseModel> RefreshToken(string refreshToken);

        // Làm mới Access Token với Google Refresh Token
        Task<AuthResponseModel> RefreshGoogleToken(string refreshToken);
    }
}
