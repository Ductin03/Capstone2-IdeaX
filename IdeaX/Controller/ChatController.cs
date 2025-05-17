using IdeaX.Attributes;
using IdeaX.ChatHub;
using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IdeaX.Controller
{
    [Route("v1/api/client[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHubService> _hubContext;

        public ChatController(IChatService chatService, IHubContext<ChatHubService> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Get chat history between two users
        /// HTTP GET: v1/api/client/chat/{senderId}/{receiverId}
        /// </summary>
        /// <param name="senderId">The sender's user ID</param>
        /// <param name="receiverId">The receiver's user ID</param>
        /// <returns>A list of chat messages between the sender and receiver</returns>
        [CustomAuthorize(RoleRequestModel.Admin, RoleRequestModel.Investor, RoleRequestModel.Investor)]
        [HttpGet("{senderId}/{receiverId}")]
        public async Task<IActionResult> GetMessages([FromQuery] Guid senderId, Guid receiverId)
        {
            var messages = await _chatService.GetChatHistory(senderId, receiverId);
            return Ok(messages);
        }

        /// <summary>
        /// Send a chat message to a receiver
        /// HTTP POST: v1/api/client/chat
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns>A success message after sending the message</returns>
        [CustomAuthorize(RoleRequestModel.Admin, RoleRequestModel.Investor, RoleRequestModel.Investor)]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
        {
            if (message == null)
                return BadRequest("Tin nhắn không hợp lệ!");

            await _chatService.SendMessage(message);
            await _hubContext.Clients.User(message.ReceiverId.ToString()).SendAsync("ReceiveMessage", message.SenderId, message.Content, message.CreatedOn);
            await _hubContext.Clients.User(message.SenderId.ToString()).SendAsync("ReceiveMessage", message.SenderId, message.Content, message.CreatedOn);
            return Ok(new { Message = "Gửi tin nhắn thành công!" });
        }
    }
}
