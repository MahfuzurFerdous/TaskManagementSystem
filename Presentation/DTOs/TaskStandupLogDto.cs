namespace TaskManagementSystem.Application.DTOs
{
    public class TaskStandupLogDto
    {
        public int Id { get; set; }
        public string? SubmittedBy { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string? Note { get; set; }
    }
}
