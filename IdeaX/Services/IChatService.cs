using IdeaX.Entities;
using IdeaX.Model.RequestModels;

namespace IdeaX.Services
{
    public interface IChatService
    {
        Task<List<ChatMessage>> GetChatHistory(Guid senderId, Guid receiverId);
        Task SendMessage(ChatMessage request);


    }
}
