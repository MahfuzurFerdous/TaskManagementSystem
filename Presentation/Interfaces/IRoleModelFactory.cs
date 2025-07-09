using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface IRoleModelFactory
    {
        Task<RoleAssignmentDto> PrepareRoleAssignmentDtoAsync(string userId);
    }
}
