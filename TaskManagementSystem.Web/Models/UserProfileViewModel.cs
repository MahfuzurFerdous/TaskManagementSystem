using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class UserProfileViewModel
    {
        public string FullName { get; set; }

        [Display(Name = "Bio")]
        public string Bio { get; set; }

        [Display(Name = "Avatar URL")]
        public IFormFile? AvatarFile { get; set; }
        public string AvatarUrl { get; set; }
    }

}
