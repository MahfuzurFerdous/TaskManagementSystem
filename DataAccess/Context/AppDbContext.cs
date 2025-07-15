using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DataAccess.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TaskCard> TaskCards { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<TaskStandupLog> TaskStandupLogs { get; set; }
        public DbSet<QueuedEmail> QueuedEmails { get; set; }
    }
}