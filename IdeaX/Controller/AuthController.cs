using IdeaX.Model.RequestModels;
using IdeaX.Response;
using IdeaX.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IdeaX.Model.ResponseModels;

namespace IdeaX.Controller
{
    [Route("v1/api/client/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        private readonly IConfiguration _configuration;
        public AuthController(IAuth authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }
        /// <summary>
        /// Log in to the system
        /// HTTP GET: v1/api/client/Auth
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            try
            {
                var auth = await _authService.Authentication(request);
                return Ok(auth);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var response = await _authService.RefreshToken(request.RefreshToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        // Khởi tạo quá trình xác thực Google
        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback)),
                Items =
            {
                { "returnUrl", Url.Content("~/") }
            }
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Endpoint được gọi khi Google trả về kết quả xác thực
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Cookies");

            if (!authenticateResult.Succeeded)
            {
                return Unauthorized();
            }

            // Lấy access_token, refresh_token được lưu tự động
            var accessToken = await HttpContext.GetTokenAsync("Cookies", "access_token");
            var refreshToken = await HttpContext.GetTokenAsync("Cookies", "refresh_token");

            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Google access token không khả dụng");
            }

            try
            {
                // Gửi accessToken sang AuthService để xác thực và sinh JWT
                var authResult = await _authService.GoogleAuthentication(accessToken);

                // Lưu refreshToken nếu cần
                //if (!string.IsNullOrEmpty(refreshToken))
                //{
                //    await _authService.RefreshGoogleToken(, refreshToken);
                //}

                // Chuyển hướng về client app
                var clientUrl = _configuration["Authentication:ClientApp:Url"];
                return Redirect($"{clientUrl}?access_token={authResult.AccessToken}&refresh_token={authResult.RefreshToken}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        // Endpoint để làm mới token Google
        [HttpPost("refresh-google-token")]
        public async Task<IActionResult> RefreshGoogleToken([FromBody] RefreshGoogleTokenRequest request)
        {
            try
            {
                var response = await _authService.RefreshGoogleToken(request.GoogleRefreshToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        //[HttpGet]
        //public IActionResult GetUserInfo()
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var email = User.FindFirst(ClaimTypes.Email)?.Value;
        //    var roleName = User.FindFirst("RoleName")?.Value;

        //    return Ok(new
        //    {
        //        UserId = userId,
        //        Email = email,
        //        RoleName = roleName,
        //        IsAuthenticated = User.Identity.IsAuthenticated
        //    });
        }
    }

