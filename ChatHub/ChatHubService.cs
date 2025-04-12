using IdeaX.Entities;
using IdeaX.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace IdeaX.ChatHub
{
    public class ChatHubService : Hub
    {
        private readonly IChatService _chatService;
        private static readonly ConcurrentDictionary<Guid, string> _userConnections = new ConcurrentDictionary<Guid, string>();

        public ChatHubService(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendPrivateMessage(Guid senderId, Guid receiverId, string messageContent)
        {
            var createdOn = DateTime.UtcNow;

            Console.WriteLine($"📩 Backend Received: From {senderId} To {receiverId} - {messageContent} at {createdOn}");

            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, messageContent, createdOn);

            var message = new ChatMessage
            {
                SenderId = senderId, // Chuyển Guid thành string nếu ChatMessage dùng string
                ReceiverId = receiverId, // Chuyển Guid thành string nếu ChatMessage dùng string
                Content = messageContent,
                CreatedOn = DateTime.UtcNow
            };

            await _chatService.SendMessage(message);

            if (_userConnections.ContainsKey(receiverId))
            {
                await Clients.Client(_userConnections[receiverId]).SendAsync("ReceiveUserMessage", senderId, messageContent, message.CreatedOn);
            }
            await Clients.Caller.SendAsync("ReceiveUserMessage", senderId, messageContent, message.CreatedOn);
        }

        public override async Task OnConnectedAsync()
        {
            
            var userIdString = Context.User?.Identity?.Name;
            
            if (Guid.TryParse(userIdString, out Guid Id))
            {
                _userConnections[Id] = Context.ConnectionId;
            }
            else
            {
                Console.WriteLine($"Lỗi: Không thể chuyển đổi userId '{userIdString}' thành Guid.");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
           
            if (Guid.TryParse(Context.User?.Identity?.Name, out Guid Id))
            {
                if (Id != Guid.Empty)
                {
                    _userConnections.TryRemove(Id, out _);
                }
            }
            else
            {
                Console.WriteLine("Lỗi: Không thể chuyển đổi userId thành Guid.");
            }
           
            await base.OnDisconnectedAsync(exception);
        }
    }   
}