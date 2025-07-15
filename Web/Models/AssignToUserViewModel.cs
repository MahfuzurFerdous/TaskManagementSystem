using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagementSystem.Web.Models
{
    public class AssignToUserViewModel
    {
        public int TaskCardId { get; set; }
        public string AssignedToUserName { get; set; } = string.Empty;
        public List<SelectListItem> AvailableUsers { get; set; } = new();
        public string TaskTitle { get; set; } = string.Empty;
        public string CurrentAssignee { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; } = DateTime.Now;
    }

}
