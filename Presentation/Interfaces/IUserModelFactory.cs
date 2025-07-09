using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface IUserModelFactory
    {
        ApplicationUser Create(string fullName, string email);
        Task<UserListDto> PrepareIndexAsync(int pageNumber = 1, int pageSize = 10);
        Task<UserDto> PrepareDetailsAsync(string id);
    }

}
