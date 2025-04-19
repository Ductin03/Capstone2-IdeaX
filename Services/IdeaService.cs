using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Model.ResponseModels;
using IdeaX.Response;
using IdeaX.UnitOfWork;

namespace IdeaX.Services
{
    public class IdeaService : IIdeaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CloudinaryService _cloudinaryService;
        public IdeaService(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Responses> CreateIdea(CreateIdeaRequestModel request)
        {
            var categoryExist = await _unitOfWork.CategoryRepository.FindByIdAsync(request.CategoryId);
            if(categoryExist == null)
            {
                return new Responses(false, "khong tim thay danh muc");
            }
            var userExist = await _unitOfWork.UserRepository.FindByIdAsync(request.InitiatorId);
            if (userExist == null)
            {
                return new Responses(false, "khong tim thay user");
            }
            string imageUrls = null!;
            if (request.ImageUrls!= null)
            {
                imageUrls = await _cloudinaryService.UploadImageAsync(request.ImageUrls);
            }

            var idea = new Idea
            { 
                Id = Guid.NewGuid(),
                CategoryId = request.CategoryId,
                CommunityId = request.CommunityId,
                InitiatorId = request.InitiatorId,
                CollaborationType = request.CollaborationType,
                CopyrightCertificate = request.CopyrightCertificate,
                CopyrightStatus = request.CopyrightStatus,
                ImageUrls = imageUrls,
                isPublic = request.IsPublic,
                IdeaCode = "IDEA-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                Status = request.Status,
                Description = request.Description,
                CreatedBy = request.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                Price = request.Price,
                IsForSale = request.IsForSale,
                Title = request.Title
            };
            await _unitOfWork.IdeaRepository.CreateAsync(idea);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, "them thanh cong");
        }

        public async Task<Responses> DeleteIdea(Guid id)
        {
            var ideaExist = await _unitOfWork.IdeaRepository.FindByIdAsync(id);
            if (ideaExist == null)
            {
                return new Responses(false, "khong tim thay");
            }
            ideaExist.IsDeleted = true;
            await _unitOfWork.IdeaRepository.DeleteAsync(ideaExist);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, "xoa thanh cong");
        }

        public async Task<BasePaginationResponseModel<GetIdeaResponseModel>> GetAllIdeasPrivateForInvestor(GetIdeaRequestModel request)
        {
            return await _unitOfWork.IdeaRepository.GetAllIdeaPrivateForInvestorAsync(request);
        }

        public async Task<BasePaginationResponseModel<GetIdeaResponseModel>> GetAllIdeasPublic(GetIdeaRequestModel request)
        {
            return await _unitOfWork.IdeaRepository.GetAllIdeasPublicAsync(request);
        }

        public async Task<List<GetIdeaResponseModel>> GetIdeaByCategory(Guid categoryId)
        {
            var categoryExist = await _unitOfWork.CategoryRepository.FindByIdAsync(categoryId);
            return categoryExist != null
                ? await _unitOfWork.IdeaRepository.GetIdeaByCategoryAsync(categoryId)
                : throw new Exception("Category khong ton tai");
        }

        public async Task<List<GetIdeaResponseModel>> GetIdeaByUserId(Guid userId)
        {
            var userExist = await _unitOfWork.UserRepository.FindByIdAsync(userId);
            return userExist != null
                ? await _unitOfWork.IdeaRepository.GetIdeaByUserIdAsync(userId)
                : throw new Exception("user khong ton tai");
        }

        public async Task<Responses> UpdateIdea(UpdateIdeaRequestModel request, Guid id)
        {
            var ideaExist = await _unitOfWork.IdeaRepository.FindByIdAsync(id);
            if (ideaExist == null)
            {
                return new Responses(false, "khong tim thay");
            }
            ideaExist.Title = request.Title;
            ideaExist.Price = request.Price;
            ideaExist.Description = request.Description; 
            ideaExist.CategoryId = request.CategoryId;
            ideaExist.CommunityId = request.CommunityId;
            ideaExist.Status = request.Status;
            ideaExist.CopyrightStatus = request.CopyrightStatus;
            ideaExist.CopyrightCertificate = request.CopyrightCertificate;
            ideaExist.IsForSale = request.IsForSale;
            await _unitOfWork.IdeaRepository.UpdateAsync(ideaExist);
            await _unitOfWork.SavechangeAsync();
            return new Responses(true, "cap nhat thanh cong");
        }
    }
}
