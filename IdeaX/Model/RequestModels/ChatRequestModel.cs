namespace IdeaX.Model.RequestModels
{
    public class ChatRequestModel
    {
        public string SenderId { get; set; }  // ID của người gửi
        public string ReceiverId { get; set; } // ID của người nhận
        public string Content { get; set; }
    }
}
