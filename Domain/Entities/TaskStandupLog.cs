namespace TaskManagementSystem.Domain.Entities
{
    public class TaskStandupLog
    {
        public int Id { get; set; }
        public int TaskCardId { get; set; }
        public TaskCard? TaskCard { get; set; }
        public string? SubmittedBy { get; set; }
        public string? Note { get; set; }
        public DateTime SubmittedAt { get; set; }
    }

}
