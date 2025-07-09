
namespace TaskManagementSystem.Web.Helpers
{
    public static class TaskStatusHelper
    {
        public static string GetBadgeColor(Domain.Enums.TaskStatus status)
        {
            return status switch
            {
                Domain.Enums.TaskStatus.Created => "secondary",
                Domain.Enums.TaskStatus.AssignedToUser => "info",
                Domain.Enums.TaskStatus.InProgress => "warning",
                Domain.Enums.TaskStatus.CompletionRequested => "primary",
                Domain.Enums.TaskStatus.ManagerApproved => "success",
                Domain.Enums.TaskStatus.Completed => "success",
                Domain.Enums.TaskStatus.Reassigned => "danger",
                _ => "dark"
            };
        }
    }
}
