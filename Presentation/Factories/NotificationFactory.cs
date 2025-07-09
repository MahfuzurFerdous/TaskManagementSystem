using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Factories
{
    public class NotificationFactory : INotificationFactory
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public NotificationFactory(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        public async Task<NotificationMessageDto> CreateTaskCreationMessageAsync(TaskCard taskCard)
        {
            var manager = await _userManager.FindByNameAsync(taskCard.AssignedToUserName ?? "");
            if (manager == null || string.IsNullOrEmpty(manager.Email))
                throw new InvalidOperationException("Assigned manager is invalid or missing email.");

            var templatePath = Path.Combine(_env.WebRootPath, "Templates", "Emails", "TaskCreated.html");

            var message = new NotificationMessageDto
            {
                Subject = "🆕 New Task Assigned",
                Body = await GenerateEmailBodyAsync(templatePath, new Dictionary<string, string>
                {
                    { "TaskTitle", taskCard.Title },
                    { "TaskId", taskCard.Id.ToString() },
                    { "AssignedBy", taskCard.AssignedByUserName ?? "N/A" },
                    { "CreatedAt", taskCard.CreatedAt.ToString() },
                    { "UserName", manager.FullName ?? "User" },
                    { "DueDate", taskCard.DueDate?.ToString() ?? "N/A" } 
                }),
                RecipientEmails = new List<string> { manager.Email }
            };

            return message;
        }

        public async Task<NotificationMessageDto> CreateCompletionApprovedMessageAsync(TaskCard taskCard)
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var managers = await _userManager.GetUsersInRoleAsync("Manager");

            var recipients = admins.Concat(managers)
                .Where(u => !string.IsNullOrEmpty(u.Email))
                .GroupBy(u => u.Email)
                .Select(g => g.First())
                .ToList();

            var templatePath = Path.Combine(_env.WebRootPath, "Templates", "Emails", "TaskCompletionApproved.html");

            var message = new NotificationMessageDto
            {
                Subject = "✅ Task Completion Approved",
                Body = await GenerateEmailBodyAsync(templatePath, new Dictionary<string, string>
            {
                { "TaskTitle", taskCard.Title },
                { "TaskId", taskCard.Id.ToString() },
                { "UserName", recipients.FirstOrDefault()?.FullName ?? "Team" }
            }),
                RecipientEmails = recipients.Select(r => r.Email!).ToList()
            };

            return message;
        }

        public async Task<NotificationMessageDto> CreateCompletionRequestMessageAsync(ApplicationUser requester, TaskCard taskCard)
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            var recipients = new List<ApplicationUser>(admins);

            if (!string.IsNullOrEmpty(taskCard.AssignedByUserName))
            {
                var manager = await _userManager.FindByNameAsync(taskCard.AssignedByUserName);
                if (manager != null && !string.IsNullOrEmpty(manager.Email))
                    recipients.Add(manager);
            }

            var uniqueRecipients = recipients
                .Where(u => !string.IsNullOrEmpty(u.Email))
                .GroupBy(u => u.Email)
                .Select(g => g.First())
                .ToList();

            var templatePath = Path.Combine(_env.WebRootPath, "Templates", "Emails", "CompletionRequest.html");

            var userName = requester.FullName ?? requester.UserName ?? string.Empty;
            var message = new NotificationMessageDto
            {
                Subject = "📝 Completion Request Submitted",
                Body = await GenerateEmailBodyAsync(templatePath, new Dictionary<string, string>
                {
                    { "UserName", userName },
                    { "TaskTitle", taskCard.Title },
                    { "TaskId", taskCard.Id.ToString() }
                }),
                RecipientEmails = uniqueRecipients.Select(r => r.Email!).ToList() 
            };

            return message;
        }


        public async Task<NotificationMessageDto> CreateRoleAssignmentEmailAsync(ApplicationUser user, List<string> roles)
        {
            var templatePath = Path.Combine(_env.WebRootPath, "Templates", "Emails", "RoleAssigned.html");
            var template = await File.ReadAllTextAsync(templatePath);

            var userName = user.FullName ?? user.UserName ?? string.Empty; 
            var replacements = new Dictionary<string, string>
            {
                { "UserName", userName },
                { "Roles", string.Join(", ", roles) }
            };

            foreach (var kv in replacements)
            {
                template = template.Replace($"@{kv.Key}", kv.Value);
            }

            return new NotificationMessageDto
            {
                RecipientEmails = new List<string> { user.Email ?? string.Empty },
                Subject = "🔐 New Role Assigned",
                Body = template
            };
        }
        public async Task<NotificationMessageDto> CreateTaskAssignmentMessageAsync(TaskCard taskCard)
        {
            if (string.IsNullOrEmpty(taskCard.AssignedToUserName))
                throw new ArgumentException("Assigned user is missing");

            var user = await _userManager.FindByNameAsync(taskCard.AssignedToUserName);
            if (user == null || string.IsNullOrEmpty(user.Email))
                throw new ArgumentException("Assigned user email not found");

            var templatePath = Path.Combine(_env.WebRootPath, "Templates", "Emails", "TaskAssigned.html");

            var replacements = new Dictionary<string, string>
            {
                { "UserName", user.FullName ?? user.UserName ?? "User" },
                { "TaskTitle", taskCard.Title },
                { "TaskId", taskCard.Id.ToString() }
            };

            var body = await GenerateEmailBodyAsync(templatePath, replacements);

            return new NotificationMessageDto
            {
                Subject = "📌 New Task Assigned",
                Body = body,
                RecipientEmails = new List<string> { user.Email }
            };
        }

        public async Task<NotificationMessageDto> CreateTaskRejectionMessageAsync(ApplicationUser user, TaskCard taskCard, string reason)
        {
            var templatePath = Path.Combine(_env.WebRootPath, "Templates", "Emails", "TaskRejection.html");

            var body = await GenerateEmailBodyAsync(templatePath, new Dictionary<string, string>
            {
                { "UserName", user.FullName ?? user.UserName ?? "User" },
                { "TaskTitle", taskCard.Title },
                { "TaskId", taskCard.Id.ToString() },
                { "Reason", reason }
            });

            return new NotificationMessageDto
            {
                Subject = $"❌ Task Completion Rejected (ID: {taskCard.Id})",
                Body = body,
                RecipientEmails = new List<string> { user.Email ?? "" }
            };
        }
        public async Task<NotificationMessageDto> CreateTaskReassignmentMessageAsync(ApplicationUser user, TaskCard taskCard)
        {
            var templatePath = Path.Combine(_env.WebRootPath, "Templates", "Emails", "TaskReassigned.html");

            var body = await GenerateEmailBodyAsync(templatePath, new Dictionary<string, string>
        {
            { "UserName", user.FullName ?? user.UserName ?? "User" },
            { "TaskTitle", taskCard.Title },
            { "TaskId", taskCard.Id.ToString() }
        });

            return new NotificationMessageDto
            {
                Subject = $"🔁 Task Reassigned to You (ID: {taskCard.Id})",
                Body = body,
                RecipientEmails = new List<string> { user.Email ?? "" }
            };
        }


        private async Task<string> GenerateEmailBodyAsync(string templatePath, Dictionary<string, string> replacements)
        {
            var template = await File.ReadAllTextAsync(templatePath);

            foreach (var kv in replacements)
            {
                template = template.Replace($"@{kv.Key}", kv.Value);
            }

            return template;
        }
    }


}
