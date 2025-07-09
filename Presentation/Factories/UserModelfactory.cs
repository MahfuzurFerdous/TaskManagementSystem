using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Factories
{
    public class UserModelFactory : IUserModelFactory
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserModelFactory(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser Create(string fullName, string email)
        {
            return new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                UserName = email
            };
        }

        public async Task<UserListDto> PrepareIndexAsync(int pageNumber = 1, int pageSize = 10)
        {
            var totalCount = await _userManager.Users.CountAsync();

            var users = await _userManager.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userDtos.Add(new UserDto
                {
                    Id = user.Id ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName ?? string.Empty,
                    Roles = roles.ToList()
                });
            }

            return new UserListDto
            {
                Users = userDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }



        public async Task<UserDto> PrepareDetailsAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID '{id}' not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                Id = user.Id ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName ?? string.Empty,
                Roles = roles.ToList()
            };
        }
    }

}


