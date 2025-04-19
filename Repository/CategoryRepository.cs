using IdeaX.Entities;
using IdeaX.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdeaX.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IdeaXDbContext _context;
        public CategoryRepository(IdeaXDbContext context)
        {
            _context = context;
        }
        public async Task<Responses> CreateAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            return new Responses(true, "them thanh cong");
        }

        public async Task<Responses> DeleteAsync(Category entity)
        {
            _context.Categories.Update(entity);
            return new Responses(true, "xoa thanh cong");
        }

        public async Task<Category> FindByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public Task<Category> GetByAsync(Expression<Func<Category, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Responses> UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            return new Responses(true, "cap nhat thanh cong");
        }
    }
}
