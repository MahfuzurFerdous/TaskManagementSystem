using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationFactory _notificationFactory;
        private readonly IQueuedEmailService _queuedEmailService;

        public NotificationService(INotificationFactory notificationFactory, IQueuedEmailService queuedEmailService)
        {
            _notificationFactory = notificationFactory;
            _queuedEmailService = queuedEmailService;
        }

        public async Task NotifyTaskCreationAsync(TaskCard taskCard)
        {
            var message = await _notificationFactory.CreateTaskCreationMessageAsync(taskCard);

            foreach (var email in message.RecipientEmails)
            {
                var queuedEmail = new QueuedEmail
                {
                    To = email,
                    Subject = message.Subject,
                    Body = message.Body
                };

                await _queuedEmailService.EnqueueEmailAsync(queuedEmail);
            }
        }

        public async Task NotifyTaskCompletionAsync(TaskCard taskCard)
        {
            var message = await _notificationFactory.CreateCompletionApprovedMessageAsync(taskCard);

            foreach (var email in message.RecipientEmails)
            {
                var queuedEmail = new QueuedEmail
                {
                    To = email,
                    Subject = message.Subject,
                    Body = message.Body
                };

                await _queuedEmailService.EnqueueEmailAsync(queuedEmail);
            }
        }

        public async Task NotifyCompletionRequestAsync(ApplicationUser requester, TaskCard taskCard)
        {
            var message = await _notificationFactory.CreateCompletionRequestMessageAsync(requester, taskCard);

            foreach (var email in message.RecipientEmails)
            {
                var queuedEmail = new QueuedEmail
                {
                    To = email,
                    Subject = message.Subject,
                    Body = message.Body
                };

                await _queuedEmailService.EnqueueEmailAsync(queuedEmail);
            }
        }

        public async Task NotifyRoleAssignmentAsync(ApplicationUser user, List<string> roles)
        {
            var message = await _notificationFactory.CreateRoleAssignmentEmailAsync(user, roles);

            foreach (var recipient in message.RecipientEmails)
            {
                var queuedEmail = new QueuedEmail
                {
                    To = recipient,
                    Subject = message.Subject,
                    Body = message.Body
                };

                await _queuedEmailService.EnqueueEmailAsync(queuedEmail);
            }
        }

        public async Task NotifyTaskAssignmentAsync(TaskCard taskCard)
        {
            var message = await _notificationFactory.CreateTaskAssignmentMessageAsync(taskCard);

            foreach (var email in message.RecipientEmails)
            {
                var queuedEmail = new QueuedEmail
                {
                    To = email,
                    Subject = message.Subject,
                    Body = message.Body
                };

                await _queuedEmailService.EnqueueEmailAsync(queuedEmail);
            }
        }

        public async Task NotifyTaskRejectionAsync(ApplicationUser recipient, TaskCard taskCard, string reason)
        {
            var message = await _notificationFactory.CreateTaskRejectionMessageAsync(recipient, taskCard, reason);

            foreach (var email in message.RecipientEmails)
            {
                var queuedEmail = new QueuedEmail
                {
                    To = email,
                    Subject = message.Subject,
                    Body = message.Body,
                    CreatedAt = DateTime.UtcNow
                };

                await _queuedEmailService.EnqueueEmailAsync(queuedEmail);
            }
        }

        public async Task NotifyTaskReassignmentAsync(ApplicationUser newUser, TaskCard taskCard)
        {
            var message = await _notificationFactory.CreateTaskReassignmentMessageAsync(newUser, taskCard);

            foreach (var email in message.RecipientEmails)
            {
                var queuedEmail = new QueuedEmail
                {
                    To = email,
                    Subject = message.Subject,
                    Body = message.Body,
                    CreatedAt = DateTime.UtcNow
                };

                await _queuedEmailService.EnqueueEmailAsync(queuedEmail);
            }
        }

    }

}
