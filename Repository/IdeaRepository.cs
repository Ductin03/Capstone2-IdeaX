using IdeaX.Entities;
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

        public Task<Idea> GetByAsync(Expression<Func<Idea, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Responses> UpdateAsync(Idea entity)
        {
             _context.Ideas.Update(entity);
            return new Responses(true, "cap nhat thanh cong");
        }
    }
}
