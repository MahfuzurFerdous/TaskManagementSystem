namespace TaskManagementSystem.Application.DTOs
{
    public class NotificationMessageDto
    {
        public List<string> RecipientEmails { get; set; } = new();
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
        public string? From { get; set; }
    }

}
