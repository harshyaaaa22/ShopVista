namespace ShopVista.Models.ChatBotModel
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsFromBot { get; set; }
        public DateTime Timestamp { get; set; }
        public string SessionId { get; set; }
    }
}
