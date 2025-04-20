using IdeaX.Entities;
using IdeaX.interfaces;
using IdeaX.Model.RequestModels;
using IdeaX.Model.ResponseModels;
using IdeaX.Response;

namespace IdeaX.Repository
{
    public interface IUserRepository :IGenericInterface<User>
    {
        Task<bool> CheckIfUserExistAsync(string username);
        Task<User> CheckIfEmailExistAsync(string email);
        Task<UserResponseModel> GetInfoUserByIdAsync(Guid id);
        Task<Verification> FindEmailByOtp(string otp);
        Task<BasePaginationResponseModel<UserResponseModel>> GetAllUserAsync(GetUserRequestModel model);
        Task<Responses> CreateInvestorPreferencesAsync(User user);
    }
}
