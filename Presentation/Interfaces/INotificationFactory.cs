using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface INotificationFactory
    {
        Task<NotificationMessageDto> CreateCompletionApprovedMessageAsync(TaskCard taskCard);
        Task<NotificationMessageDto> CreateCompletionRequestMessageAsync(ApplicationUser requester, TaskCard taskCard);
        Task<NotificationMessageDto> CreateRoleAssignmentEmailAsync(ApplicationUser user, List<string> roles);
        Task<NotificationMessageDto> CreateTaskCreationMessageAsync(TaskCard taskCard);
        Task<NotificationMessageDto> CreateTaskAssignmentMessageAsync(TaskCard taskCard);
        Task<NotificationMessageDto> CreateTaskRejectionMessageAsync(ApplicationUser user, TaskCard taskCard, string reason);
        Task<NotificationMessageDto> CreateTaskReassignmentMessageAsync(ApplicationUser user, TaskCard taskCard);
    }

}
