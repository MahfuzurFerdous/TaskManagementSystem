using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagementSystem.Web.Models
{
    public class CreateTaskCardViewModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AssignedToUserName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }
        public List<SelectListItem> AvailableUsers { get; set; } = new();
    }

}
