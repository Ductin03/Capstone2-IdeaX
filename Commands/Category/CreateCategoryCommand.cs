using IdeaX.Model.RequestModels;
using IdeaX.Response;
using MediatR;

namespace IdeaX.Commands.Category
{
    public class CreateCategoryCommand : IRequest<Responses>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
