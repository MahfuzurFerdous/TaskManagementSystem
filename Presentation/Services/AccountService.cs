using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserModelFactory _userModelFactory;
        private readonly IUserProfileService _userProfileService;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserModelFactory userModelFactory, IUserProfileService userProfileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userModelFactory = userModelFactory;
            _userProfileService = userProfileService;
        }
        public async Task<(bool Success, string? ErrorMessage)> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return (false, "No account found with this email.");

            if (string.IsNullOrEmpty(user.UserName))
                return (false, "User account is invalid. Missing username.");

            if (!await _userManager.CheckPasswordAsync(user, password))
                return (false, "Invalid password.");

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
                return (true, null);

            return (false, "Login failed. Please try again.");
        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(string fullName, string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return (false, "An account with this email already exists.");

            var user = _userModelFactory.Create(fullName, email);

            var result = await _userManager.CreateAsync(user, password);

            await _userProfileService.CreateProfileForUserAsync(user.Id, fullName);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errorMessages);
            }

            await _userManager.AddToRoleAsync(user, "User");

            return (true, null);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }

}
