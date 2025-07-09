using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "User ID is required.")]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; } = null!;

        [MinLength(1, ErrorMessage = "At least one role must be assigned.")]
        public List<string> Roles { get; set; } = new();
    }
}
