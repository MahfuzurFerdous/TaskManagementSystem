using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Application.DTOs
{
    public class TaskCardSearchDto
    {
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string? Title { get; set; }

        [Display(Name = "Assigned User")]
        [StringLength(50, ErrorMessage = "Assigned user name cannot exceed 50 characters.")]
        public string? AssignedToUserName { get; set; }

        [Display(Name = "Status")]
        public Domain.Enums.TaskStatus? Status { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than zero.")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;
    }
}
