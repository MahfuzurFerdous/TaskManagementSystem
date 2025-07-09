using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TaskManagementSystem.Application.DTOs
{
    public class UserProfileDto
    {
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; }

        [Display(Name = "Bio")]
        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters.")]
        public string Bio { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? AvatarFile { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
