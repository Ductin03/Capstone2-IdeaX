using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Response;
using IdeaX.UnitOfWork;

namespace IdeaX.Services
{
    public class IdeaService : IIdeaService
    {
        private readonly IUnitOfWork _unitOfWork;
        public IdeaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Responses> CreateIdea(CreateIdeaRequestModel request)
        {
            var categoryExist = await _unitOfWork.CategoryRepository.FindByIdAsync(request.CategoryId);
            if(categoryExist == null)
            {
                return new Responses(false, "khong tim thay");
            }
            var idea = new Idea
            { 
                Id = Guid.NewGuid(),
                CategoryId = request.CategoryId,
                CommunityId = request.CommunityId,
                CopyrightCertificate = request.CopyrightCertificate,
                CopyrightStatus = request.CopyrightStatus,
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

        public async Task<List<Idea>> GetAllIdea()
        {
            return await _unitOfWork.IdeaRepository.GetAllAsync();
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
