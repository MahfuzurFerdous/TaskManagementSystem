namespace TaskManagementSystem.Web.Models
{
    public class RoleAssignmentModel
    {
        public string UserId { get; set; } = null!;
        public List<string> AvailableRoles { get; set; } = new();
        public List<string> SelectedRoles { get; set; } = new();
        public DateTime RoleChangedAt { get; set; } = DateTime.Now;
    }
}
