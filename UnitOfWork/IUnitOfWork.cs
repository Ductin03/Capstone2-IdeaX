using IdeaX.Entities;
using IdeaX.Repository;

namespace IdeaX.UnitOfWork
{
    public interface IUnitOfWork : IDisposable 
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IdeaXDbContext Context { get; }
        Task SavechangeAsync();
    }
}
