using IdeaX.Entities;
using IdeaX.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;

namespace IdeaX.ChatHub
{
    public class AIChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private static readonly Guid AI_USER_ID = Guid.Parse("00000000-0000-0000-0000-000000000001");

        public AIChatHub(IChatService chatService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _chatService = chatService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task SendMessageToAI(Guid senderId, string messageContent)
        {
            var message = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = AI_USER_ID,
                Content = messageContent,
                CreatedOn = DateTime.UtcNow
            };

            await _chatService.SendMessage(message);
            await Clients.Caller.SendAsync("ReceiveAIMessage", senderId, messageContent, message.CreatedOn);

            string aiResponse = await GetAIResponse(messageContent);

            var aiMessage = new ChatMessage
            {
                SenderId = AI_USER_ID,
                ReceiverId = senderId,
                Content = aiResponse,
                CreatedOn = DateTime.UtcNow
            };

            await _chatService.SendMessage(aiMessage);
            await Clients.Caller.SendAsync("ReceiveAIMessage", AI_USER_ID, aiResponse, aiMessage.CreatedOn);
        }

        private async Task<string> GetAIResponse(string message)
        {
            var client = _httpClientFactory.CreateClient();
            var apiKey = _configuration["AI:ApiKey"];
            var requestBody = new
            {
                prompt = message,
                max_tokens = 50
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                var response = await client.PostAsync("https://api.example.com/v1/completions", content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<dynamic>(json);
                    return result.choices[0].text.ToString();
                }
                return "AI không thể phản hồi lúc này.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gọi API AI: {ex.Message}");
                return "Có lỗi xảy ra khi liên lạc với AI.";
            }
        }
    }
}