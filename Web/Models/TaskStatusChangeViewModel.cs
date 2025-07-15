namespace TaskManagementSystem.Web.Models
{
    public class TaskStatusChangeViewModel
    {
        public Domain.Enums.TaskStatus Status { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? Remarks { get; set; } 
    }

}
