using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task CreateProfileForUserAsync(string userId, string fullName);
        Task UpdateProfileAsync(string userId, UserProfileDto dto, string webRootPath);
        Task<UserProfileDto> GetProfileForEditAsync(string userId);
    }

}
