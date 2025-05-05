using IdeaX.Entities;
using IdeaX.Repository;

namespace IdeaX.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IIdeaRepository _ideaRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IdeaXDbContext _context;
        public UnitOfWork(IUserRepository userRepository,
                            IRoleRepository roleRepository,
                            IIdeaRepository ideaRepository,
                            ICategoryRepository categoryRepository,
                            IdeaXDbContext context
            )

        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _ideaRepository = ideaRepository;
            _categoryRepository = categoryRepository;
            _context = context;
        }
        public IUserRepository UserRepository => _userRepository;

        public IRoleRepository RoleRepository => _roleRepository;

        public IIdeaRepository IdeaRepository => _ideaRepository;
        public ICategoryRepository CategoryRepository => _categoryRepository;

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
