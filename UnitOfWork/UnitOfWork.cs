using IdeaX.Entities;
using IdeaX.Repository;

namespace IdeaX.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUserRepository _userRepository;
        private readonly IdeaXDbContext _context;
        private readonly IRoleRepository _roleRepository;
        public UnitOfWork(IUserRepository userRepository,
                            IRoleRepository roleRepository,
                            IdeaXDbContext context
            )

        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _context = context;
        }
        public IUserRepository UserRepository => _userRepository;

        public IRoleRepository RoleRepository => _roleRepository;

        public IdeaXDbContext Context => _context;


        public async Task SavechangeAsync()
        {
            await _context.SaveChangesAsync();  
        }
        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
