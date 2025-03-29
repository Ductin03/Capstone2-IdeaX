using IdeaX.Response;
using System.Linq.Expressions;

namespace IdeaX.interfaces
{
    public interface IGenericInterface<T> where T : class
    {
        Task<Responses> CreateAsync(T entity);
        Task<Responses> UpdateAsync(T entity);
        Task<Responses> DeleteAsync(T entity);
        Task<T> FindByIdAsync(Guid id);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
    }
}
