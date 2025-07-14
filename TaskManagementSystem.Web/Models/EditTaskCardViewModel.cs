using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagementSystem.Web.Models
{
    public class EditTaskCardViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title field is required.")]
        [StringLength(100, ErrorMessage = "The Title must be at most 100 characters.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        [StringLength(500, ErrorMessage = "The Description must be at most 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please select a user.")]
        public string? AssignedToUserName { get; set; }

        [Required(ErrorMessage = "The Due Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }
        public DateTime? EditedAt { get; set; } = DateTime.Now;
        public Domain.Enums.TaskStatus Status { get; set; }
        public List<SelectListItem> AvailableUsers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StatusList { get; set; } = new();
        //public List<SelectListItem> AssignedToUserList { get; set; } = new();

    }

}
