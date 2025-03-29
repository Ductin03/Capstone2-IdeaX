using IdeaX.Entities;

namespace IdeaX.Repository
{
    public interface IChatRepository
    {
        Task SaveMessageAsync(ChatMessage message);
        Task<List<ChatMessage>> GetMessageAsync(Guid senderId, Guid receiverId);

    }
}
