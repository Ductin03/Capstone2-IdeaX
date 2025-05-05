using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Model.ResponseModels;
using IdeaX.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdeaX.Repository
{
    public class IdeaRepository : IIdeaRepository
    {
        private readonly IdeaXDbContext _context;
        public IdeaRepository(IdeaXDbContext context)
        {
            _context = context;
        }
        public async Task<Responses> CreateAsync(Idea entity)
        {
            await _context.Ideas.AddAsync(entity);
            return new Responses(true, "them thanh cong");
        }

        public async Task<Responses> DeleteAsync(Idea entity)
        {
             _context.Ideas.Update(entity);
            return new Responses(true, "xoa thanh cong");
        }

        public async Task<Idea> FindByIdAsync(Guid id)
        {
            var ideaExist = await _context.Ideas.FirstOrDefaultAsync(x => x.Id == id);
            return ideaExist;
        }

        public Task<List<Idea>> GetAllAsync()
        {
           return _context.Ideas.ToListAsync();
        }

        public async Task<BasePaginationResponseModel<GetIdeaResponseModel>> GetAllIdeaPrivateForInvestorAsync(GetIdeaRequestModel model)
        {
            var query = from i in _context.Ideas
                        join initiator in _context.Users on i.InitiatorId equals initiator.Id
                        join c in _context.Categories on i.CategoryId equals c.Id

                        join investor in _context.Users on i.InvestorId equals investor.Id into investorGroup
                        from investor in investorGroup.DefaultIfEmpty()
                        where i.isPublic == false
                        select new GetIdeaResponseModel
                        {
                            Title = i.Title,
                            CopyrightCertificate = i.CopyrightCertificate,
                            Category = c.Name,
                            CopyrightStatus = i.CopyrightStatus,
                            Description = i.Description,
                            IdeaCode = i.IdeaCode,
                            Initiator = initiator.FullName,
                            InvestmentDate = i.InvestmentDate,
                            Investor = investor != null ? investor.FullName : null, // Null check for investor
                            IsForSale = i.IsForSale,
                            Price = i.Price,
                            Status = i.Status,
                            TotalComments = i.TotalComments,
                            TotalLikes = i.TotalLikes,
                            TotalRatings = i.TotalRatings,
                            TotalViews = i.TotalViews,
                            CreatedBy = i.CreatedBy,
                            CreatedOn = i.CreatedOn,
                            Id = i.Id,
                            IsDeleted = i.IsDeleted,
                            UpdatedBy = i.UpdatedBy,
                            UpdatedOn = i.UpdatedOn
                        };

            if (!string.IsNullOrWhiteSpace(model.Keyword))
            {
                query = query.Where(x => x.Title.ToLower().Contains(model.Keyword.ToLower()));
            }
            query = query.OrderByDescending(x => x.CreatedOn);
            var total = await query.CountAsync();

            if (model.PageSize > 0)
            {
                query = query.Skip(model.PageIndex * model.PageSize).Take(model.PageSize);
            }
            var items = await query.ToListAsync();
            return new BasePaginationResponseModel<GetIdeaResponseModel>(model.PageIndex, model.PageSize, total, items);
        }

        public async Task<BasePaginationResponseModel<GetIdeaResponseModel>> GetAllIdeasPublicAsync(GetIdeaRequestModel model)
        {
            var query = from i in _context.Ideas
                        join initiator in _context.Users on i.InitiatorId equals initiator.Id
                        join c in _context.Categories on i.CategoryId equals c.Id
                        
                        join investor in _context.Users on i.InvestorId equals investor.Id into investorGroup
                        from investor in investorGroup.DefaultIfEmpty() 
                        where i.isPublic == true
                        select new GetIdeaResponseModel
                        {
                            Title = i.Title,
                            CopyrightCertificate = i.CopyrightCertificate,
                            Category = c.Name,
                            CopyrightStatus = i.CopyrightStatus,
                            Description = i.Description,
                            IdeaCode = i.IdeaCode,
                            Initiator = initiator.FullName,
                            InvestmentDate = i.InvestmentDate,
                            Investor = investor != null ? investor.FullName : null, // Null check for investor
                            IsForSale = i.IsForSale,
                            Price = i.Price,
                            Status = i.Status,
                            TotalComments = i.TotalComments,
                            TotalLikes = i.TotalLikes,
                            TotalRatings = i.TotalRatings,
                            TotalViews = i.TotalViews,
                            CreatedBy = i.CreatedBy,
                            CreatedOn = i.CreatedOn,
                            Id = i.Id,
                            IsDeleted = i.IsDeleted,
                            UpdatedBy = i.UpdatedBy,
                            UpdatedOn = i.UpdatedOn
                        };

            if (!string.IsNullOrWhiteSpace(model.Keyword))
            {
                query = query.Where(x => x.Title.ToLower().Contains(model.Keyword.ToLower()));
            }
            query = query.OrderByDescending(x => x.CreatedOn);
            var total = await query.CountAsync();

            if (model.PageSize > 0)
            {
                query = query.Skip(model.PageIndex * model.PageSize).Take(model.PageSize);
            }
            var items = await query.ToListAsync();
            return new BasePaginationResponseModel<GetIdeaResponseModel>(model.PageIndex, model.PageSize, total, items);
            
        }

        public Task<Idea> GetByAsync(Expression<Func<Idea, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetIdeaResponseModel>> GetIdeaByCategoryAsync(Guid categoryId)
        {
            var query = from i in _context.Ideas
                        join c in _context.Categories on i.CategoryId equals c.Id
                        join u in _context.Users on i.InitiatorId equals u.Id

                        join investor in _context.Users on i.InvestorId equals investor.Id into investorGroup
                        from investor in investorGroup.DefaultIfEmpty()
                        where i.CategoryId == categoryId
                        select new GetIdeaResponseModel
                        {
                            Title = i.Title,
                            CopyrightCertificate = i.CopyrightCertificate,
                            Category = c.Name,
                            CopyrightStatus = i.CopyrightStatus,
                            Description = i.Description,
                            IdeaCode = i.IdeaCode,
                            Initiator = u.FullName,
                            InvestmentDate = i.InvestmentDate,
                            Investor = investor != null ? investor.FullName : null, // Null check for investor
                            IsForSale = i.IsForSale,
                            Price = i.Price,
                            Status = i.Status,
                            TotalComments = i.TotalComments,
                            TotalLikes = i.TotalLikes,
                            TotalRatings = i.TotalRatings,
                            TotalViews = i.TotalViews,
                            CreatedBy = i.CreatedBy,
                            CreatedOn = i.CreatedOn,
                            Id = i.Id
                        };
            return await query.ToListAsync();
        }

        public async Task<List<GetIdeaResponseModel>> GetIdeaByUserIdAsync(Guid userId)
        {
            var query = from i in _context.Ideas
                        join u in _context.Users on i.InitiatorId equals u.Id
                        join c in _context.Categories on i.CategoryId equals c.Id

                        join investor in _context.Users on i.InvestorId equals investor.Id into investorGroup
                        from investor in investorGroup.DefaultIfEmpty()
                        where i.InitiatorId == userId
                        select new GetIdeaResponseModel
                        {
                            Category = c.Name,
                            CopyrightCertificate = i.CopyrightCertificate,
                            CopyrightStatus = i.CopyrightStatus,
                            CreatedBy = i.CreatedBy,
                            CreatedOn = i.CreatedOn,
                            ImageUrls = i.ImageUrls,
                            Description = i.Description,
                            IdeaCode = i.IdeaCode,
                            Initiator = u.FullName,
                            Price = i.Price,
                            Title = i.Title,
                            TotalComments = i.TotalComments,
                            TotalLikes = i.TotalLikes,
                            TotalRatings = i.TotalRatings,
                            TotalViews = i.TotalViews,
                            Status = i.Status,
                            UpdatedBy = i.UpdatedBy,
                            UpdatedOn = i.UpdatedOn,                   
                        };
            var ideas = await query.ToListAsync();
            return ideas;
        }

        public async Task<Responses> UpdateAsync(Idea entity)
        {
             _context.Ideas.Update(entity);
            return new Responses(true, "cap nhat thanh cong");
        }
    }
}
