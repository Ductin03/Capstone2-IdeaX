using IdeaX.Attributes;
using IdeaX.Model.RequestModels;
using IdeaX.Response;
using IdeaX.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaX.Controller
{
    [Route("v1/api/client/[controller]")]
    [Authorize(RoleRequestModel.Admin, RoleRequestModel.Investor, RoleRequestModel.Investor)]
    [ApiController]
    public class IdeasController : ControllerBase
    {
        private readonly IIdeaService _ideaService;
        public IdeasController(IIdeaService ideaService)
        {
            _ideaService = ideaService;
        }

        /// <summary>
        /// Create a new idea
        /// HTTP POST: v1/api/client/ideas
        /// </summary>
        /// <param name="request">The request containing the idea details</param>
        /// <returns>A response indicating success or failure</returns>
        [HttpPost]
        public async Task<ActionResult<Responses>> CreateIdea([FromBody] CreateIdeaRequestModel request)
        {
            var response = await _ideaService.CreateIdea(request);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Update an existing idea
        /// HTTP PATCH: v1/api/client/ideas/{id}
        /// </summary>
        /// <param name="request">The request containing updated idea details</param>
        /// <param name="id">The ID of the idea to update</param>
        /// <returns>A response indicating success or failure</returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<Responses>> UpdateIdea([FromBody] UpdateIdeaRequestModel request, Guid id)
        {
            var response = await _ideaService.UpdateIdea(request, id);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Get all public ideas
        /// HTTP GET: v1/api/client/ideas
        /// </summary>
        /// <param name="request">The request for retrieving the ideas</param>
        /// <returns>A list of public ideas</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllIdeasPublic([FromQuery] GetIdeaRequestModel request)
        {
            var ideas = await _ideaService.GetAllIdeasPublic(request);
            return Ok(ideas);
        }

        /// <summary>
        /// Get all private ideas for investors
        /// HTTP GET: v1/api/client/ideas/for-investor
        /// </summary>
        /// <param name="request">The request for retrieving the ideas</param>
        /// <returns>A list of private ideas for investors</returns>
        [HttpGet("for-investor")]
        public async Task<IActionResult> GetAllIdeasPrivateForInvestor([FromQuery] GetIdeaRequestModel request)
        {
            var ideas = await _ideaService.GetAllIdeasPrivateForInvestor(request);
            return Ok(ideas);
        }

        /// <summary>
        /// Delete an idea by its ID
        /// HTTP DELETE: v1/api/client/ideas/{id}
        /// </summary>
        /// <param name="id">The ID of the idea to delete</param>
        /// <returns>A response indicating success or failure</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Responses>> RemoveIdea(Guid id)
        {
            var response = await _ideaService.DeleteIdea(id);
            return response.Flag ? BadRequest(response) : Ok(response);
        }

        /// <summary>
        /// Get ideas by user ID
        /// HTTP GET: v1/api/client/ideas/find-by-userid
        /// </summary>
        /// <param name="userId">The user ID for which to fetch the ideas</param>
        /// <returns>A list of ideas created by the specified user</returns>
        [HttpGet("find-by-userid")]
        public async Task<IActionResult> GetIdeaByUserId([FromQuery] Guid userId)
        {
            var ideas = await _ideaService.GetIdeaByUserId(userId);
            return Ok(ideas);
        }

        /// <summary>
        /// Get ideas by category ID
        /// HTTP GET: v1/api/client/ideas/find-by-categoryid
        /// </summary>
        /// <param name="categoryId">The category ID for which to fetch the ideas</param>
        /// <returns>A list of ideas within the specified category</returns>
        [HttpGet("find-by-categoryid")]
        public async Task<IActionResult> GetIdeaByCategory([FromQuery] Guid categoryId)
        {
            var ideas = await _ideaService.GetIdeaByCategory(categoryId);
            return Ok(ideas);
        }
    }
}
