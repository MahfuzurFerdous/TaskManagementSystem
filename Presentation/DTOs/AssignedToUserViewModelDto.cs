using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagementSystem.Application.DTOs
{
    public class AssignToUserViewModelDto
    {
        public int TaskCardId { get; set; }

        public string AssignedToUserName { get; set; } = string.Empty;
        public string AssignedByUserName { get; set; } = string.Empty;
        public List<SelectListItem> AvailableUsers { get; set; } = new();
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public string TaskTitle { get; set; } = string.Empty;

        public string CurrentAssignee { get; set; } = string.Empty;
    }

}
