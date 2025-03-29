using IdeaX.Model.RequestModels;

namespace IdeaX.Services
{
    public interface IAuth
    {
        Task<string> Authentication(LoginRequestModel request);
    }
}
