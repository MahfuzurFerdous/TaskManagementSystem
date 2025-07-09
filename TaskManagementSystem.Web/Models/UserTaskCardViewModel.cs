
namespace TaskManagementSystem.Web.Models
{
    public class UserTaskCardViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsStarted { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public bool IsRequestedForCompletion { get; set; }
        public string? DailyStandupNote { get; set; }
        public DateTime? LastStandupUpdatedAt { get; set; }
    }

}
