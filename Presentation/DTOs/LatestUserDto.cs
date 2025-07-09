namespace TaskManagementSystem.Application.DTOs
{
    public class LatestUserDto
    {
        public string FullName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = "/images/default-avatar.png";
        public DateTime RegistrationDate { get; set; }
    }
}
