using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers;

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserModelFactory _userModelFactory;
    private readonly IMapper _mapper;
    private readonly IUserProfileService _profileService;
    private readonly IUserProfileRepository _profileRepository;
    private readonly IWebHostEnvironment _env;

    public UserController(UserManager<ApplicationUser> userManager, IUserModelFactory userModelFactory, IMapper mapper, IUserProfileService profileService, IUserProfileRepository userProfileRepository, IWebHostEnvironment env)
    {
        _userManager = userManager;
        _userModelFactory = userModelFactory;
        _mapper = mapper;
        _profileService = profileService;
        _profileRepository = userProfileRepository;
        _env = env;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
    {
        var dto = await _userModelFactory.PrepareIndexAsync(pageNumber, pageSize);

        var model = new UserListViewModel
        {
            Users = _mapper.Map<List<UserViewModel>>(dto.Users),
            PageNumber = dto.PageNumber,
            PageSize = dto.PageSize,
            TotalCount = dto.TotalCount
        };

        return View(model);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null) await _userManager.DeleteAsync(user);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin,User,Manager")]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);

        var dto = await _profileService.GetProfileForEditAsync(user.Id);


        var model = _mapper.Map<UserProfileViewModel>(dto);

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Profile(UserProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);

        var dto = _mapper.Map<UserProfileDto>(model);

        await _profileService.UpdateProfileAsync(user.Id, dto, _env.WebRootPath);

        TempData["Success"] = "Profile updated successfully.";
        return RedirectToAction("Profile");
    }

}
