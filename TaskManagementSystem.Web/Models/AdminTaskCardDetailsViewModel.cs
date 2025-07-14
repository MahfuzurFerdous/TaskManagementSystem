namespace TaskManagementSystem.Web.Models
{
    public class AdminTaskCardDetailsViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? AssignedTo { get; set; }
        public string? AssignedBy { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletionRequestedAt { get; set; }
        public DateTime? ManagerApprovedAt { get; set; }
        public DateTime? AdminApprovedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public int CycleCount { get; set; }
        public DateTime ReassignedAt { get; internal set; }
    }

}
