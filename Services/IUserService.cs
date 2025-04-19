using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Model.ResponseModels;
using IdeaX.Response;

namespace IdeaX.Services
{
    public interface IUserService
    {
        Task<Responses> Register(UserRequestModel model);
        Task<Responses> VerifyOTP(string otp, string email);
        Task<User> FindById(Guid id);
        Task<UserResponseModel> GetInfoUserById(Guid id);
        Task<Responses> Update(Guid id, UpdateUserRequestModel model);
        Task SendEmail(MailRequestModel model);
        Task<Responses> ForgotPassword(string email);
        Task<Responses> ResetPassword(string otp, string password, string confirmPassword);
        Task<BasePaginationResponseModel<UserResponseModel>> GetAllUser(GetUserRequestModel request);
    }
}
