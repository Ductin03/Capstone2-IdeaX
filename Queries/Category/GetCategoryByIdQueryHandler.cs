using IdeaX.Entities;
using IdeaX.Repository;
using MediatR;

namespace IdeaX.Queries.Category
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, IdeaX.Entities.Category>
    {
        private readonly ICategoryRepository _categoryRepository;
        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<Entities.Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.FindByIdAsync(request.CategoryId);
            if (category == null)
            {
                return null!;
            }
            return category;
        }
    }
}
