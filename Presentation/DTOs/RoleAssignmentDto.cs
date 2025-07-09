namespace TaskManagementSystem.Application.DTOs
{
    public class RoleAssignmentDto
    {
        public string? UserId { get; set; }

        public List<string> AvailableRoles { get; set; }
        public List<string> SelectedRoles { get; set; }
        public DateTime RoleChangedAt { get; set; } = DateTime.Now;

        public RoleAssignmentDto()
        {
            AvailableRoles = new List<string>();
            SelectedRoles = new List<string>();
        }
    }

}
