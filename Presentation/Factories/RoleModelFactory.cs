using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Factories
{
    public class RoleModelFactory : IRoleModelFactory
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleModelFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<RoleAssignmentDto> PrepareRoleAssignmentDtoAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User not found.");
            }

            var allRoles = _roleManager.Roles.Select(r => r.Name!).ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

            return new RoleAssignmentDto
            {
                UserId = userId,
                AvailableRoles = allRoles,
                SelectedRoles = userRoles.ToList()
            };
        }
    }
}
