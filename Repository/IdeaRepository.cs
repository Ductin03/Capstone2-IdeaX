using IdeaX.Entities;
using IdeaX.Response;
using System.Linq.Expressions;

namespace IdeaX.Repository
{
    public class IdeaRepository : IIdeaRepository
    {
        public Task<Responses> CreateAsync(Idea entity)
        {
            throw new NotImplementedException();
        }

        public Task<Responses> DeleteAsync(Idea entity)
        {
            throw new NotImplementedException();
        }

        public Task<Idea> FindByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Idea>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Idea> GetByAsync(Expression<Func<Idea, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Responses> UpdateAsync(Idea entity)
        {
            throw new NotImplementedException();
        }
    }
}
