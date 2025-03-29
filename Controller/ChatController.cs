using IdeaX.ChatHub;
using IdeaX.Entities;
using IdeaX.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IdeaX.Controller
{
    [Route("api/[controller]")]
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

        [HttpGet("{senderId}/{receiverId}")]
        public async Task<IActionResult> GetMessages([FromQuery]Guid senderId, Guid receiverId)
        {
            var messages = await _chatService.GetChatHistory(senderId, receiverId);
            return Ok(messages);
        }

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
