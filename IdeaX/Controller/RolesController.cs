using IdeaX.Attributes;
using IdeaX.Model.RequestModels;
using IdeaX.Response;
using IdeaX.Services;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;

namespace IdeaX.Controller
{
    [Route("v1/api/admin/[controller]")]
    [CustomAuthorize(RoleRequestModel.Admin)]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Create a new role in the system
        /// HTTP POST: v1/api/admin/roles
        /// </summary>
        /// <param name="request">The request containing the role details</param>
        /// <returns>A response indicating success or failure</returns>
        [HttpPost]
        public async Task<ActionResult<Responses>> CreateRole(CreateRoleRequestModel request)
        {
            var response = await _roleService.CreateRole(request);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Update an existing role in the system
        /// HTTP PATCH: v1/api/admin/roles/{id}
        /// </summary>
        /// <param name="id">The ID of the role to update</param>
        /// <param name="request">The request containing updated role details</param>
        /// <returns>A response indicating success or failure</returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<Responses>> UpdateRole(Guid id, UpdateRoleRequestModel request)
        {
            var response = await _roleService.UpdateRole(id, request);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Get all roles in the system
        /// HTTP GET: v1/api/admin/roles
        /// </summary>
        /// <returns>A list of all roles</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRole()
        {
            var roles = await _roleService.GetAllRole();
            return Ok(roles);
        }

        /// <summary>
        /// Delete a role by its ID
        /// HTTP DELETE: v1/api/admin/roles/{id}
        /// </summary>
        /// <param name="id">The ID of the role to delete</param>
        /// <returns>A response indicating success or failure</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Responses>> RemoveRole(Guid id)
        {
            var response = await _roleService.DeleteRole(id);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Get a specific role by its ID
        /// HTTP GET: v1/api/admin/roles/find-role/{id}
        /// </summary>
        /// <param name="id">The ID of the role to fetch</param>
        /// <returns>The details of the requested role</returns>
        [HttpGet("find-role/{id}")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            var response = await _roleService.GetRoleById(id);
            return Ok(response);
        }
        
        /// <summary>
        /// Lấy danh sách role công khai, loại trừ Admin
        /// HTTP GET: v1/api/admin/roles/public
        /// </summary>
        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<IActionResult> GetPublicRoles()
        {
            var allRoles = await _roleService.GetAllRole();
            
            // Lọc bỏ role "Admin"
            var filtered = allRoles
                .Where(r => !string.Equals(r.RoleName, "Admin", StringComparison.OrdinalIgnoreCase))
                .Select(r => new {
                    r.Id,
                    r.RoleName,
                    r.Description
                })
                .ToList();

            return Ok(filtered);
        }
    }
}
