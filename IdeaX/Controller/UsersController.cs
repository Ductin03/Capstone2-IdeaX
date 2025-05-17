using IdeaX.Attributes;
using IdeaX.Model.RequestModels;
using IdeaX.Response;
using IdeaX.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        /// <summary>
        /// Register a new user in the system
        /// HTTP POST: v1/api/client/users
        /// </summary>
        /// <param name="request">The request containing user details</param>
        /// <returns>A response indicating success or failure</returns>
        [HttpPost]
        public async Task<ActionResult<Responses>> Register([FromBody] UserRequestModel request)
        {
            var response = await _userService.Register(request);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Verify OTP for a user during registration or password reset
        /// HTTP POST: v1/api/client/users/verify-otp
        /// </summary>
        /// <param name="model">The OTP verification model containing OTP and email</param>
        /// <returns>A response indicating success or failure</returns>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationRequestModel model)
        {
            var response = await _userService.VerifyOTP(model.OTP, model.Email);
            return Ok(response);
        }

        /// <summary>
        /// Get user information by their ID
        /// HTTP GET: v1/api/client/users/{id}
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>The user's information</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> InfoUser(Guid id)
        {
            return Ok(await _userService.GetInfoUserById(id));
        }

        /// <summary>
        /// Get user information by their ID from query parameters
        /// HTTP GET: v1/api/client/users/get-by-id
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>The user's information</returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetUserById([FromQuery] Guid id)
        {
            return Ok(await _userService.FindById(id));
        }

        /// <summary>
        /// Update user information
        /// HTTP PATCH: v1/api/client/users/{id}
        /// </summary>
        /// <param name="id">The ID of the user to update</param>
        /// <param name="request">The request containing updated user details</param>
        /// <returns>A response indicating success or failure</returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<Responses>> UpdateUser(Guid id, UpdateUserRequestModel request)
        {
            var response = await _userService.Update(id, request);
            return response.Flag ? Ok(request) : BadRequest(response);
        }

        /// <summary>
        /// Request a password reset for the user
        /// HTTP POST: v1/api/client/users/forgot-password
        /// </summary>
        /// <param name="email">The email address of the user requesting a password reset</param>
        /// <returns>A response indicating success or failure</returns>
        [HttpPost("forgot-password")]
        public async Task<ActionResult<Responses>> ForgotPassword(string email)
        {
            var response = await _userService.ForgotPassword(email);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Reset the user's password using OTP
        /// HTTP PATCH: v1/api/client/users/reset-password
        /// </summary>
        /// <param name="otp">The OTP for password reset</param>
        /// <param name="password">The new password</param>
        /// <param name="confirmPassword">The confirmation of the new password</param>
        /// <returns>A response indicating success or failure</returns>
        [HttpPatch("reset-password")]
        public async Task<ActionResult<Responses>> ResetPassword(string otp, string password, string confirmPassword)
        {
            var response = await _userService.ResetPassword(otp, password, confirmPassword);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Get all users with pagination and filters (admin, investor, founder access)
        /// HTTP GET: v1/api/client/users
        /// </summary>
        /// <param name="request">The request model containing pagination and filter parameters</param>
        /// <returns>A list of users based on the request</returns>
        [CustomAuthorize(RoleRequestModel.Admin, RoleRequestModel.Investor, RoleRequestModel.Founder)]
        [HttpGet]
        public async Task<IActionResult> GetAllUser([FromQuery] GetUserRequestModel request)
        {
            return Ok(await _userService.GetAllUser(request));
        }

        /// <summary>
        /// Create or update investor preferences for the authenticated user
        /// HTTP PATCH: v1/api/client/users
        /// </summary>
        /// <param name="request">The request model containing investor preference details</param>
        /// <returns>A response indicating success or failure of the operation</returns>
        [HttpPatch]
        public async Task<ActionResult<Responses>> CreateInvestorPreferencesAsync(CreateInvestorPreferencesRequestModel request)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(Guid.TryParse(user, out Guid userId))
            {
               var response = await _userService.CreateInvestorPreferencesAsync(request, userId);
               return response.Flag ? Ok(response) : BadRequest(response);  
            }
            return Unauthorized(new Responses
            {
                Flag = false,
                Message = "Unauthorized"
            });

        }
    }
}
