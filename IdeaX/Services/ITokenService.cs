using System.Security.Claims;

namespace IdeaX.Services
{
    public interface ITokenService
    {
        public string CreateAccessToken(Claim[] claims);
        public string CreateRefreshToken();
    }
}
