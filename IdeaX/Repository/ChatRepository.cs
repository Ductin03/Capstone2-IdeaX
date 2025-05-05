using IdeaX.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdeaX.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly IdeaXDbContext _context;
        public ChatRepository(IdeaXDbContext context)
        {
            _context = context;
        }
        public async Task<List<ChatMessage>> GetMessageAsync(Guid senderId, Guid receiverId)
        {
            return await _context.Messages.
                Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId)
                                || (m.SenderId == receiverId && m.ReceiverId == senderId)).
                OrderBy(m => m.CreatedOn).
                ToListAsync();
        }

        public async Task SaveMessageAsync(ChatMessage message)
        {
            await _context.Messages.AddAsync(message);
        }
    }
}
