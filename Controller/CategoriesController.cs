using IdeaX.Attributes;
using IdeaX.Commands.Category;
using IdeaX.Model.RequestModels;
using IdeaX.Queries.Category;
using IdeaX.Response;
using IdeaX.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaX.Controller
{
    [Route("v1/api/admin/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new category
        /// HTTP POST: v1/api/admin/category
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(RoleRequestModel.Admin)]
        [HttpPost]
        public async Task<ActionResult<Responses>> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            var response = await _mediator.Send(command);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Get a category by its ID
        /// HTTP GET: v1/api/admin/category/{id}
        /// </summary>
        /// <param name="id">The ID of the category</param>
        /// <returns></returns>
        [Authorize(RoleRequestModel.Admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var query = new GetCategoryByIdQuery { CategoryId = id };
            var result = await _mediator.Send(query);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Update category
        /// HTTP PACTH: v1/api/admin/category/{id}
        /// </summary>
        /// <param name="id">The ID of the category</param>
        /// <returns></returns>
        //[Authorize(RoleRequestModel.Admin)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryRequestModel request, Guid id)
        {
            var response = await _categoryService.UpdateCategory(request, id);
            return response.Flag ? Ok(response) : BadRequest(response)  ; 
        }

        /// <summary>
        /// Update category
        /// HTTP PACTH: v1/api/admin/category/{id}
        /// </summary>
        /// <param name="id">The ID of the category</param>
        /// <returns></returns>
        //[Authorize(RoleRequestModel.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCategory(Guid id)
        {
            var response = await _categoryService.RemoveCategory(id);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

    }
}
