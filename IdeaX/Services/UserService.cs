using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Model.ResponseModels;
using IdeaX.Response;
using IdeaX.UnitOfWork;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;  
using System.Net.Mail;
using System.Text.Json;
using Microsoft.Extensions.Options;


namespace IdeaX.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CloudinaryService _cloudinaryService;
        private readonly EmailSettingModel _emailSettingModel;
        private readonly EmailCacheService _emailCacheService;
        public UserService(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService, IOptions<EmailSettingModel> options, EmailCacheService emailCacheService)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
            _emailSettingModel = options.Value;
            _emailCacheService = emailCacheService;
        }

        public Task<User> FindById(Guid id)
        {
            return _unitOfWork.UserRepository.FindByIdAsync(id);
        }

        public async Task<UserResponseModel> GetInfoUserById(Guid id)
        {
            var response = await _unitOfWork.UserRepository.GetInfoUserByIdAsync(id);
            if (response == null)
            {
                throw new Exception("User not exist!");
            }
            return response;
        }

        public async Task<Responses> Register(UserRequestModel model)
        {
            try
            {
                
                string otp = new Random().Next(100000, 999999).ToString();

                var userExist = await _unitOfWork.UserRepository.CheckIfUserExistAsync(model.Username);

                if (userExist)
                {
                    return new Responses(false, "Tài khoản đã được sử dụng");
                }
                if (model.Password != model.ConfirmPassword)
                {
                    return new Responses(false, "Mật khẩu xác nhận không khớp");
                }
                var emailExist = await _unitOfWork.UserRepository.CheckIfEmailExistAsync(model.Email);
                if (emailExist != null)
                {
                    return new Responses(false, "Email đã được sử dụng");

                }
                //if (model.CCCDBack != null)
                //{
                //    using (var stream = model.CCCDBack.OpenReadStream())
                //    {
                //        ccccBack = await _cloudinaryService.UploadImageAsync(stream, Guid.NewGuid().ToString());
                //    }
                //}
                //if (model.CCCDFront != null)
                //{
                //    using (var stream = model.CCCDFront.OpenReadStream()) 
                //    {
                //        ccccFront = await _cloudinaryService.UploadImageAsync(stream, Guid.NewGuid().ToString());
                //    }
                //}
                //if (model.Avatar != null)
                //{
                //    using (var stream = model.Avatar.OpenReadStream()) 
                //    {
                //        avatar = await _cloudinaryService.UploadImageAsync(stream, Guid.NewGuid().ToString());
                //    }
                //}
                //string cccdBackUrl = await _cloudinaryService.UploadImageAsync(model.CCCDBack);
                //string cccdFrontUrl = await _cloudinaryService.UploadImageAsync(model.CCCDFront);
                //string avatarUrl = await _cloudinaryService.UploadImageAsync(model.Avatar);
                var roleExist = await _unitOfWork.RoleRepository.FindByIdAsync(model.RoleId);

                if (roleExist == null)
                {
                    return new Responses(false, $"Vai trò này không có sẵn");
                }
                if (string.IsNullOrEmpty(model.Password) || model.Password.Length < 8)
                {
                    return new Responses(false, "Mật khẩu phải lớn hơn 8 ký tự");
                }
                if (model.Birthday > DateTime.Today)
                {
                    return new Responses(false, "Ngày sinh không được lớn hơn ngày hiện tại");
                }
                var jsonData = JsonSerializer.Serialize(model);

                var verification = new Verification
                {
                    Id = Guid.NewGuid(),
                    Email = model.Email,
                    OTP = otp,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(3),
                    Data = jsonData
                };

                //var user = new User()
                //{
                //    Id = Guid.NewGuid(),
                //    Username = model.Username,
                //    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                //    Email = model.Email,
                //    Phone = model.Phone,
                //    RoleId = model.RoleId,
                //    Address = model.Address,
                //    Avatar = avatar!,
                //    Birthday = model.Birthday,
                //    CCCD = model.CCCD,
                //    CCCDBack = ccccBack!,
                //    CCCDFront= ccccFront!,
                //    CreatedBy = model.CreatedBy,
                //    CreatedOn = DateTime.UtcNow,
                //    IsDeleted = false,
                //};
                await _unitOfWork.Context.Verifications.AddAsync(verification);
                await _unitOfWork.SavechangeAsync();

                var emailBody = GeneralEmailBody(model.Username, otp);
                await SendEmail(new MailRequestModel
                {
                    Email = model.Email,
                    Subject = "Xác nhận tài khoản IdeaX",
                    EmailBody = emailBody
                });

                return new Responses(true, "Một mã OTP đã được gửi tới Email. Vui lòng xác nhận để đăng ký");
            }
            catch (Exception ex)
            {
                return new Responses(false, $"Có lỗi xảy ra trong quá trình xử lý: {ex.Message}");
            }
        }

        public async Task SendEmail(MailRequestModel model)
        {
            
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(model.Email);
            email.To.Add(MailboxAddress.Parse(model.Email));
            email.Subject = model.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = model.EmailBody;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_emailSettingModel.Host, _emailSettingModel.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettingModel.Email, _emailSettingModel.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public string GeneralEmailBody(string name, string otpText)
        {
            string emailBody = string.Empty;
            emailBody = "<div style='width=100%; background-color:grey'>";
            emailBody += "<h1>Hi " + name + ", Forget OTP</h1>";
            emailBody += "<h2>Please Enter Otp  and Complete Password Reset</h2>";
            emailBody += "<h2>OTP text: " + otpText + "</h2>";
            emailBody += "</div>";

            return emailBody;
        }
        public async Task<Responses> Update(Guid id, UpdateUserRequestModel model)
        {
            var userExist = await _unitOfWork.UserRepository.FindByIdAsync(id);
            if (userExist == null)
            {
                return new Responses(false, "User not exist");
            }
            if (!string.IsNullOrEmpty(model.FullName))
                userExist.FullName = model.FullName;

            if (!string.IsNullOrEmpty(model.Email))  
                userExist.Email = model.Email;

            if (!string.IsNullOrEmpty(model.CCCD))
                userExist.CCCD = model.CCCD;

            if (!string.IsNullOrEmpty(model.Phone))
                userExist.Phone = model.Phone;

            if (!string.IsNullOrEmpty(model.Avatar))
                userExist.Avatar = model.Avatar;

            if (!string.IsNullOrEmpty(model.Address))
                userExist.Address = model.Address;

            if (!string.IsNullOrEmpty(model.CCCDBack))
                userExist.CCCDBack = model.CCCDBack;

            if (!string.IsNullOrEmpty(model.CCCDFront))
                userExist.CCCDFront = model.CCCDFront;

            
            userExist.Birthday = model.Birthday;

            await _unitOfWork.UserRepository.UpdateAsync(userExist);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, "Update successed");
        }

        public async Task<Responses> VerifyOTP(string otp, string email)
        {
            try
            {
                var verification = await _unitOfWork.Context.Verifications
                    .FirstOrDefaultAsync(x => x.OTP == otp && x.Email == email);

                if (verification == null || verification.ExpirationTime < DateTime.UtcNow)
                {
                    return new Responses(false, "OTP không đúng hoặc đã quá hạn!");
                }
                var model = JsonSerializer.Deserialize<UserRequestModel>(verification.Data);
                if (email != model!.Email)
                {
                    return new Responses(false, "Vui lòng nhập đúng email ở bước đăng ký");
                }

                // Chuyển JSON về đối tượng UserRequestModel


                // Upload ảnh lên Cloudinary
                string cccdBackUrl = await _cloudinaryService.UploadImageAsync(model.CCCDBack);
                string cccdFrontUrl = await _cloudinaryService.UploadImageAsync(model.CCCDFront);
                //string avatar = await _cloudinaryService.UploadImageAsync(model.Avatar);
                // Lưu tài khoản vào database
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = model.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Email = model.Email,
                    FullName = model.FullName,
                    Phone = model.Phone,
                    RoleId = model.RoleId,
                    Address = model.Address,
                    Avatar = "",
                    Birthday = model.Birthday,
                    CCCD = model.CCCD,
                    CCCDBack = cccdBackUrl,
                    CCCDFront = cccdFrontUrl,
                    CreatedBy = model.CreatedBy,
                    Token = "",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                };

                await _unitOfWork.UserRepository.CreateAsync(user);
                await _unitOfWork.SavechangeAsync();

                // Xóa OTP sau khi xác thực thành công

                _unitOfWork.Context.Verifications.Remove(verification);
                await _unitOfWork.SavechangeAsync();

                return new Responses(true, "Tài khoản đã được tạo thành công");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message}"); Console.WriteLine($"Error: {ex.InnerException?.Message}");
                return new Responses(false, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<Responses> ForgotPassword(string email)
        {
            var emailExist = await _unitOfWork.UserRepository.CheckIfEmailExistAsync(email);
            if(emailExist == null )
            {
                return new Responses(false, "Email khong ton tai!");
            }
            var mailRequest = new MailRequestModel();
            mailRequest.Email = email;
            mailRequest.Subject = "Forget Password : OTP";
            var otpText = new Random().Next(100000, 999999).ToString();
            mailRequest.EmailBody = GeneralEmailBody(email, otpText);
            await SendEmail(mailRequest);
            var verification = new Verification();
            verification.Id = Guid.NewGuid();
            verification.Email = email;
            verification.OTP = otpText;
            verification.ExpirationTime = DateTime.UtcNow.AddSeconds(90);
            await _unitOfWork.Context.Verifications.AddAsync(verification);
            await _unitOfWork.SavechangeAsync();
     
            return new Responses(true, $"Đã gửi otp đến bạn {email}");
        }

        public async Task<Responses> ResetPassword(string otp, string password, string confirmPassword)
        {
            var emailExist = await _unitOfWork.UserRepository.FindEmailByOtp(otp);
            if(emailExist.Email == null) 
            {
                return new Responses(false, "Otp sai hoac da het han");
                    }
            var userExist = await _unitOfWork.UserRepository.CheckIfEmailExistAsync(emailExist.Email);
            if(userExist == null)
            {
                return new Responses(false, "not exist");
            }
            if( password != confirmPassword )
            {
                return new Responses(false, "Mat khau xac nhan khong dung");
            }
            userExist.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            await _unitOfWork.UserRepository.UpdateAsync(userExist);
            _unitOfWork.Context.Verifications.Remove(emailExist);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, "Xac thuc thanh cong");


        }

        public async Task<BasePaginationResponseModel<UserResponseModel>> GetAllUser(GetUserRequestModel request)
        {
            return await _unitOfWork.UserRepository.GetAllUserAsync(request);
        }

        public async Task<Responses> CreateInvestorPreferencesAsync(CreateInvestorPreferencesRequestModel request, Guid userId)
        {
            var userExist = await _unitOfWork.UserRepository.FindByIdAsync(userId);
            if(userExist == null)
            {
                return new Responses(false, "Khon ton tai user");
            }
            userExist.FundingRangeMax = request.FundingRangeMax;
            userExist.FundingRangeMin = request.FundingRangeMin;
            userExist.PreferredIndustries = request.PreferredIndustries;
            userExist.PreferredRegions = request.PreferredRegions;
            userExist.PreferredStages = request.PreferredStages;
            await _unitOfWork.UserRepository.UpdateAsync(userExist);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, "Them thanh cong");
        }
    }
}
