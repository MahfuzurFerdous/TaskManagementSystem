namespace TaskManagementSystem.Application.DTOs
{
    public class UserTaskCardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsStarted { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsRequestedForCompletion { get; set; }
    }
}
