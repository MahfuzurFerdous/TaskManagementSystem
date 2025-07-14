namespace TaskManagementSystem.Web.Models
{
    public class TaskStandupLogViewModel
    {
        public int Id { get; set; }
        public string? SubmittedBy { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string? Note { get; set; }
    }
}
