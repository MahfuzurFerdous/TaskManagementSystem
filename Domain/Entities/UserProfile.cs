namespace TaskManagementSystem.Domain.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }

}
