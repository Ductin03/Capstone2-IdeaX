using IdeaX.Attributes;
using IdeaX.Model.RequestModels;
using IdeaX.Response;
using IdeaX.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaX.Controller
{
    [Route("v1/api/client/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<ActionResult<Responses>> Register([FromBody]UserRequestModel request)
        {
            var response = await _userService.Register(request);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationRequestModel model)
        {
            var response = await _userService.VerifyOTP(model.OTP, model.Email);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> InfoUser([FromQuery] Guid id) //làm tạm, lấy id ở token làm inforUser
        {
            return Ok(await _userService.GetInfoUserById(id));
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetUserById([FromQuery] Guid id)
        {
            return Ok(await _userService.FindById(id));
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<Responses>> UpdateUser(Guid id, UpdateUserRequestModel request)
        {
            var response = await _userService.Update(id, request);
            return response.Flag ? Ok(request) : BadRequest(response);
        }
        [HttpPost("forgot-password")]
        public async Task<ActionResult<Responses>> ForgotPassword(string email)
        {
            var response = await _userService.ForgotPassword(email);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
        [HttpPatch("reset-password")]
        public async Task<ActionResult<Responses>> ResetPassword(string otp, string password, string confirmPassword)
        {
            var response = await _userService.ResetPassword(otp, password, confirmPassword);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
    }
}
