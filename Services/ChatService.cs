using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Repository;
using Microsoft.EntityFrameworkCore;

namespace IdeaX.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IdeaXDbContext _context;
        public ChatService(IChatRepository chatRepository, IdeaXDbContext context)
        {
           _chatRepository = chatRepository;
            _context = context;
        }
        public async Task<List<ChatMessage>> GetChatHistory(Guid senderId, Guid receiverId)
        {
            return await _chatRepository.GetMessageAsync(senderId, receiverId);
        }

        public async Task SendMessage(ChatMessage request)
        {
            var receiverExist = await _context.Users.FirstOrDefaultAsync( x => x.Id == request.ReceiverId );
            if (receiverExist == null)
            {
                throw new Exception("nguoi nhan khong ton tai");
            }
            var senderExist = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.SenderId);
            if (senderExist == null)
            {
                throw new Exception("nguoi gui khong ton tai");
            }
            await _chatRepository.SaveMessageAsync(request);
            await _context.SaveChangesAsync();
        }
    }
}
