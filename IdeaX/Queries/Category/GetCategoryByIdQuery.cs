using IdeaX.Entities;
using MediatR;

namespace IdeaX.Queries.Category
{
    public class GetCategoryByIdQuery : IRequest<IdeaX.Entities.Category>
    {
        public Guid CategoryId { get; set; }
    }
}
