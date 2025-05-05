namespace IdeaX.Entities
{
    public class ChatMessage : BaseEntity
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; } = false;
        public string? GroupId { get; set; }  // Null nếu không phải tin nhắn nhóm
        public override string ToString()
        {
            return $"Sender: {SenderId}, Receiver: {ReceiverId}, Content: \"{Content}\", CreatedOn: {CreatedOn}";
        }
    }
}
