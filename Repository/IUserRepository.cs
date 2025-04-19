using IdeaX.Entities;
using IdeaX.interfaces;
using IdeaX.Model.RequestModels;
using IdeaX.Model.ResponseModels;

namespace IdeaX.Repository
{
    public interface IUserRepository :IGenericInterface<User>
    {
        Task<bool> CheckIfUserExistAsync(string username);
        Task<User> CheckIfEmailExistAsync(string email);
        Task<UserResponseModel> GetInfoUserByIdAsync(Guid id);
        Task<Verification> FindEmailByOtp(string otp);
        Task<BasePaginationResponseModel<UserResponseModel>> GetAllUserAsync(GetUserRequestModel model);
    }
}
