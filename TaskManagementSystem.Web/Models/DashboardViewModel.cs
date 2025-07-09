using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Web.Models
{
    public class DashboardViewModel
    {
        public int AssignedTaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
        public int PendingTaskCount { get; set; }
        public int RegisteredUserCount { get; set; }
        public int GuestVisitorCount { get; set; } 

        public List<string> ChartLabels { get; set; } 
        public List<int> TaskCompletionData { get; set; }
        public List<CalendarEventDto> CalendarEvents { get; set; } = new();
        public List<LatestUserDto> LatestUsers { get; set; } = new();
        public List<int> MonthlyRegistrations { get; set; } = new();
        public List<string> RegistrationMonths { get; set; } = new();
        public List<int> TaskStatusCounts { get; set; } = new();
    }

}
