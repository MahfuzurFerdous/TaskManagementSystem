using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;


namespace TaskManagementSystem.Application.Services
{


    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
        }

        public async Task AssignRolesAsync(RoleAssignmentDto model, string currentUserId)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Role assignment model cannot be null.");

            if (string.IsNullOrEmpty(model.UserId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(model.UserId));

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                throw new InvalidOperationException($"User with ID '{model.UserId}' not found.");

            var currentRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, model.SelectedRoles);
        }

    }

}
