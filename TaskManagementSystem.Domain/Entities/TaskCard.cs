using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Domain.Entities
{
    public class TaskCard
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AssignedToUserName { get; set; } 
        public string? AssignedByUserName { get; set; } 
        public DateTime CreatedAt { get; set; }
        public Enums.TaskStatus Status { get; set; }

        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsRequestedForCompletion { get; set; }
        public bool IsManagerApproved { get; set; }
        public bool IsAdminApproved { get; set; }
        public bool IsRejectedByManager { get; set; }
        public bool IsRejectedByAdmin { get; set; }
        public bool IsStarted { get; set; }
        public string? ManagerApprovedBy { get; set; }
        public DateTime? ManagerApprovedAt { get; set; } 
        public string? AdminApprovedBy { get; set; }
        public DateTime? AdminApprovedAt { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletionRequestedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? ReassignedAt { get; set; }
        public string? DailyStandupNote { get; set; }
        public DateTime? LastStandupUpdatedAt { get; set; }
        public string? AdminRejectedBy { get; set; }
        public DateTime? AdminRejectedAt { get; set; }
        public string? ManagerRejectedBy { get; set; }
        public DateTime? ManagerRejectedAt { get; set; }
        public string? ManagerRejectionReason { get; set; }
        public string? AdminRejectionReason { get; set; }
       

        public virtual ICollection<TaskStandupLog> StandupLogs { get; set; } = new List<TaskStandupLog>();


    }
}
