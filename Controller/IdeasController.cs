using IdeaX.Model.RequestModels;
using IdeaX.Response;
using IdeaX.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaX.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdeasController : ControllerBase
    {
        private readonly IIdeaService _ideaService;
        public IdeasController(IIdeaService ideaService)
        {
            _ideaService = ideaService;
        }
        [HttpPost]
        public async Task<ActionResult<Responses>> CreateIdea([FromBody] CreateIdeaRequestModel request)
        {
            var response = await _ideaService.CreateIdea(request);
            return response.Flag ? Ok(response) : BadRequest(response);  
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<Responses>> UpdateIdea([FromBody] UpdateIdeaRequestModel request,Guid id)
        {
            var response = await _ideaService.UpdateIdea(request, id);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllIdea()
        {
            var idea = await _ideaService.GetAllIdea();
            return Ok(idea);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Responses>> RemoveIdea(Guid id)
        {
            var response = await _ideaService.DeleteIdea(id);
            return response.Flag ? BadRequest(response) : Ok(response); 
        }


    }
}
