using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Web.Controllers;

[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly IRoleModelFactory _roleModelFactory;
    private readonly IRoleService _roleService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationService _notificationService;

    public RoleController(IRoleModelFactory roleModelFactory, IRoleService roleService, UserManager<ApplicationUser> userManager, INotificationService notificationService)
    {
        _roleModelFactory = roleModelFactory;
        _roleService = roleService;
        _userManager = userManager;
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Assign(string userId)
    {
        var model = await _roleModelFactory.PrepareRoleAssignmentDtoAsync(userId);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Assign(RoleAssignmentDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var currentUserId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(currentUserId))
            {
                ModelState.AddModelError("", "Current user ID cannot be determined.");
                return View(model);
            }

            if (string.IsNullOrEmpty(model.UserId))
            {
                ModelState.AddModelError("", "Target user ID cannot be null or empty.");
                return View(model);
            }

            if (model.UserId == currentUserId)
            {
                ModelState.AddModelError("", "You cannot change your own role.");
                return View(model);
            }

            await _roleService.AssignRolesAsync(model, currentUserId);

            var assignedUser = await _userManager.FindByIdAsync(model.UserId);
            if (assignedUser == null)
            {
                ModelState.AddModelError("", "Assigned user not found.");
                return View(model);
            }

            var selectedRoles = model.SelectedRoles ?? new List<string>();

            await _notificationService.NotifyRoleAssignmentAsync(assignedUser, selectedRoles);

            TempData["SuccessMessage"] = "Role(s) assigned and user notified.";
            return RedirectToAction("Index", "User");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "An unexpected error occurred.");
            return View(model);
        }
    }

}



