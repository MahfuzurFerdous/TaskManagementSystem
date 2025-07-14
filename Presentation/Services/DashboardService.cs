using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.DataAccess.Context;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserProfileRepository _userProfileRepository;

        public DashboardService(AppDbContext context, UserManager<ApplicationUser> userManager, IUserProfileRepository userProfileRepository)
        {
            _context = context;
            _userManager = userManager;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<int> GetAssignedTaskCountAsync()
        {
            return await _context.TaskCards.CountAsync();
        }

        public async Task<int> GetCompletedTaskCountAsync()
        {
            return await _context.TaskCards.CountAsync(t => t.IsCompleted);
        }

        public async Task<int> GetPendingTaskCountAsync()
        {
            return await _context.TaskCards.CountAsync(t => !t.IsCompleted);
        }

        public async Task<int> GetUserCountAsync()
        {
            return await _userManager.Users.CountAsync();
        }

        public async Task<int> GetGuestVisitorCountAsync()
        {
            return await Task.FromResult(42);
        }

        public async Task<List<string>> GetChartLabelsAsync()
        {
            var months = Enumerable.Range(1, 6).Select(m =>
                new DateTime(DateTime.Now.Year, m, 1).ToString("MMM")
            ).ToList();

            return await Task.FromResult(months);
        }

        public async Task<List<int>> GetMonthlyCompletedTaskDataAsync()
        {
            var currentYear = DateTime.Now.Year;

            var data = await _context.TaskCards
                .Where(t => t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value.Year == currentYear)
                .GroupBy(t => t.DueDate!.Value.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new List<int>();

            for (int i = 1; i <= 6; i++)
            {
                var monthData = data.FirstOrDefault(x => x.Month == i);
                result.Add(monthData?.Count ?? 0);
            }

            return result;
        }

        public async Task<List<LatestUserDto>> GetLatestUsersAsync(int count = 5)
        {
            var userProfiles = await _userProfileRepository.GetAllProfilesAsync();

            return userProfiles
                .OrderByDescending(u => u.CreatedOn)
                .Take(count)
                .Select(u => new LatestUserDto
                {
                    FullName = u.FullName,
                    AvatarUrl = string.IsNullOrEmpty(u.AvatarUrl) ? "/images/default-avatar.png" : u.AvatarUrl,
                    RegistrationDate = u.CreatedOn
                })
                .ToList();
        }

        public async Task<List<CalendarEventDto>> GetCalendarEventsAsync()
        {
            return await _context.TaskCards
                .Where(t => t.DueDate != null)
                .Select(t => new CalendarEventDto
                {
                    title = t.Title,
                    start = t.DueDate.Value.ToString("yyyy-MM-dd"),
                    color = t.IsCompleted ? "#28a745" : "#ffc107"
                })
                .ToListAsync();
        }

        public async Task<List<int>> GetTaskStatusCountsAsync()
        {
            var assigned = await GetAssignedTaskCountAsync();
            var pending = await GetPendingTaskCountAsync();
            var completed = await GetCompletedTaskCountAsync();

            return new List<int> { assigned, pending, completed };
        }

        public async Task<List<string>> GetRegistrationMonthsAsync()
        {
            return await Task.Run(() =>
            {
                var pastMonths = Enumerable.Range(0, 6).Select(i =>
                    DateTime.UtcNow.AddMonths(-i).ToString("MMM")
                ).Reverse().ToList();

                return pastMonths;
            });
        }

        public async Task<List<int>> GetMonthlyUserRegistrationsAsync()
        {
            var pastMonths = Enumerable.Range(0, 6).Select(i =>
            {
                var date = DateTime.UtcNow.AddMonths(-i);
                return new { Month = date.Month, Year = date.Year };
            }).Reverse().ToList();

            var users = await _userManager.Users.ToListAsync();

            var result = new List<int>();
            foreach (var m in pastMonths)
            {
                var count = users.Count(u => u.CreatedAt.Month == m.Month && u.CreatedAt.Year == m.Year);
                result.Add(count);
            }

            return result;
        }

    }
}
