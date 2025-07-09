using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<int> GetAssignedTaskCountAsync();
        Task<int> GetCompletedTaskCountAsync();
        Task<int> GetPendingTaskCountAsync();
        Task<int> GetUserCountAsync();
        Task<int> GetGuestVisitorCountAsync();
        Task<List<string>> GetChartLabelsAsync();
        Task<List<int>> GetMonthlyCompletedTaskDataAsync();
        Task<List<LatestUserDto>> GetLatestUsersAsync(int count = 5);
        Task<List<CalendarEventDto>> GetCalendarEventsAsync();
        Task<List<int>> GetTaskStatusCountsAsync();
        Task<List<string>> GetRegistrationMonthsAsync();
        Task<List<int>> GetMonthlyUserRegistrationsAsync();

    }
}
