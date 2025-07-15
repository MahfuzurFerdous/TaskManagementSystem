using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDashboardService _dashboardService;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, IDashboardService dashboardService)
        {
            _logger = logger;
            _userManager = userManager;
            _dashboardService = dashboardService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Welcome");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin") || roles.Contains("Manager"))
                return RedirectToAction("Dashboard");


            return View("UserIndex");
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Dashboard()
        {
            var model = new DashboardViewModel
            {
                AssignedTaskCount = await _dashboardService.GetAssignedTaskCountAsync(),
                CompletedTaskCount = await _dashboardService.GetCompletedTaskCountAsync(),
                PendingTaskCount = await _dashboardService.GetPendingTaskCountAsync(),
                RegisteredUserCount = await _dashboardService.GetUserCountAsync(),
                GuestVisitorCount = await _dashboardService.GetGuestVisitorCountAsync(),
                LatestUsers = await _dashboardService.GetLatestUsersAsync(),
                CalendarEvents = await _dashboardService.GetCalendarEventsAsync(),
                ChartLabels = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun" },
                TaskCompletionData = await _dashboardService.GetMonthlyCompletedTaskDataAsync(),
                RegistrationMonths = await _dashboardService.GetRegistrationMonthsAsync(),
                TaskStatusCounts = await _dashboardService.GetTaskStatusCountsAsync(),
                MonthlyRegistrations = await _dashboardService.GetMonthlyUserRegistrationsAsync(),

            };

            return View(model);
        }


        [AllowAnonymous]
        public IActionResult Welcome()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index");
            }

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
