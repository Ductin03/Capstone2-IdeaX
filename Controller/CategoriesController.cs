using IdeaX.Commands.Category;
using IdeaX.Queries.Category;
using IdeaX.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaX.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<ActionResult<Responses>> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            var response = await _mediator.Send(command);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var query = new GetCategoryByIdQuery { CategoryId = id };
            var result = await _mediator.Send(query);
            if (result == null)
                return null!;
            return Ok(result);
        }
    }
}
