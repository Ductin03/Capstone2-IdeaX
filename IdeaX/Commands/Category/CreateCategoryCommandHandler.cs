using IdeaX.Entities;
using IdeaX.Response;
using IdeaX.UnitOfWork;
using MediatR;

namespace IdeaX.Commands.Category
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Responses>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
               _unitOfWork = unitOfWork;
        }

        public async Task<Responses> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new IdeaX.Entities.Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                CreatedBy = request.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };
            await _unitOfWork.CategoryRepository.CreateAsync(category);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, "them thanh cong");
           
        }
    }
}
