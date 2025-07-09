using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyTaskCompletionAsync(TaskCard taskCard);
        Task NotifyCompletionRequestAsync(ApplicationUser requester, TaskCard taskCard);
        Task NotifyRoleAssignmentAsync(ApplicationUser user, List<string> roles);
        Task NotifyTaskCreationAsync(TaskCard taskCard);
        Task NotifyTaskAssignmentAsync(TaskCard taskCard);
        Task NotifyTaskRejectionAsync(ApplicationUser recipient, TaskCard taskCard, string reason);
        Task NotifyTaskReassignmentAsync(ApplicationUser newUser, TaskCard taskCard);
    }

}
