using IdeaX.Attributes;
using IdeaX.Model.RequestModels;
using IdeaX.Response;
using IdeaX.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaX.Controller
{
    [Route("v1/api/admin/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        /// <summary>
        /// Administrator adds permissions to the system
        /// HTTP POST: v1/api/admin/roles
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[Authorize(RoleRequestModel.Founder)]
        [HttpPost]
        public async Task<ActionResult<Responses>> CreateRole(CreateRoleRequestModel request)
        {
            var response = await _roleService.CreateRole(request);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        //[Authorize(RoleRequestModel.Founder)]
        [HttpPatch("{id}")]
        public async Task<ActionResult<Responses>> UpdateRole(Guid id, UpdateRoleRequestModel request)
        {
            var response = await _roleService.UpdateRole(id, request);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        //[Authorize(RoleRequestModel.Founder)]
        [HttpGet]
        public async Task<IActionResult> GetAllRole()
        {
            var roles = await _roleService.GetAllRole();
            return Ok(roles);
        }

        //[Authorize(RoleRequestModel.Founder)]
        [HttpDelete("{roleId}")]
        public async Task<ActionResult<Responses>> RemoveRole(Guid roleId)
        {
            var response = await _roleService.DeleteRole(roleId);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        //[Authorize(RoleRequestModel.Founder)]
        [HttpGet("find-role/{id}")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            var response = await _roleService.GetRoleById(id);
            return Ok(response);
        }

    }
}
