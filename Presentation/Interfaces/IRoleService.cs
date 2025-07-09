using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface IRoleService
    {
        Task AssignRolesAsync(RoleAssignmentDto model, string currentUserId);
    }

}
