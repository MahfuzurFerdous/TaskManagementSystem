using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagementSystem.Application.DTOs
{
    public class CreateTaskCardDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AssignedToUserName { get; set; }
        public List<SelectListItem> AvailableUsers { get; set; } = new();
        public string? CreatedByUserName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }
    }

}
