using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagementSystem.Web.Models
{
    public class TaskCardViewModel
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public string? AssignedToUserName { get; set; }
        public string? AssignedByUserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsRequestedForCompletion { get; set; }
        public bool IsManagerApproved { get; set; }
        public string? ManagerApprovedBy { get; set; }
        public bool IsAdminApproved { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? ReassignedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletionRequestedAt { get; set; }
        public DateTime? ManagerApprovedAt { get; set; }
        public DateTime? AdminApprovedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? DailyStandupNote { get; set; }
        public DateTime? LastStandupUpdatedAt { get; set; }
        public List<SelectListItem>? AvailableUsers { get; set; }
        public string? AdminRejectionReason { get; set; }
        public string? ManagerRejectionReason { get; set; }
        public bool IsRejectedByManager { get; set; }
        public bool IsRejectedByAdmin { get; set; }
        public string? AdminRejectedBy { get; set; }
        public DateTime? AdminRejectedAt { get; set; }
        public string? ManagerRejectedBy { get; set; }
        public DateTime? ManagerRejectedAt { get; set; }
        public List<TaskStatusChangeViewModel>? StatusChanges { get; set; }
        public TaskStandupLogListModel? StandupLogList { get; set; }
        public List<SelectListItem> StatusList { get; set; } = new();
        public List<SelectListItem> AssignedToUserList { get; set; } = new();


    }
}
