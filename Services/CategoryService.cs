using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Response;
using IdeaX.UnitOfWork;

namespace IdeaX.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Responses> RemoveCategory(Guid categoryId)
        {
            var categoryExist = await _unitOfWork.CategoryRepository.FindByIdAsync(categoryId);
            if (categoryExist == null) 
            {
                return new Responses(true, "khong tim thay");
            }
            categoryExist.IsDeleted = true;
            await _unitOfWork.CategoryRepository.DeleteAsync(categoryExist);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, $"xoa thanh cong {categoryExist.Name}");
        }

        public async Task<Responses> UpdateCategory(UpdateCategoryRequestModel request, Guid categoryId)
        {
            var categoryExist = await _unitOfWork.CategoryRepository.FindByIdAsync(categoryId);
            if (categoryExist == null)
            {
                return new Responses(true, "khong tim thay");
            }
            categoryExist.Name = request.Name;
            categoryExist.Description = request.Description;
            await _unitOfWork.CategoryRepository.UpdateAsync(categoryExist);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, $"cap nhat thanh cong danh muc {categoryExist.Name}");
        }
    }
}
