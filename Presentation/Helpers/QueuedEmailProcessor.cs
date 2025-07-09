using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManagementSystem.DataAccess.Context;

namespace TaskManagementSystem.Application.Helpers
{
    public class QueuedEmailProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public QueuedEmailProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                var emails = await db.QueuedEmails
                                     .Where(e => !e.Sent)
                                     .Take(10)
                                     .ToListAsync();

                foreach (var email in emails)
                {
                    try
                    {
                        await emailSender.SendEmailAsync(email.To, email.Subject, email.Body);
                        email.Sent = true;
                        email.SentAt = DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Email send failed: {ex.Message}");
                    }
                }

                await db.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }

}
