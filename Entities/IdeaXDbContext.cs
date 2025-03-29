using Microsoft.EntityFrameworkCore;

namespace IdeaX.Entities
{
    public class IdeaXDbContext :DbContext
    {
        public IdeaXDbContext(DbContextOptions<IdeaXDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles{ get; set; }
        public DbSet<Verification> Verifications { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
    }
}
