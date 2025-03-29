using IdeaX.Model.RequestModels;
using IdeaX.Response;
using IdeaX.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaX.Controller
{
    [Route("v1/api/client/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;
        public AuthController(IAuth auth)
        {
              _auth = auth;
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
            var token = await _auth.Authentication(request);
            return Ok( new { token = token });
        } 
    }
}
