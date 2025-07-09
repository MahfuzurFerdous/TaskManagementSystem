using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Factories
{
    public class UserProfileFactory : IUserProfileFactory
    {
        public UserProfile Create(string userId, string fullName)
        {
            return new UserProfile
            {
                UserId = userId,
                FullName = fullName,
                Bio = "New User",
                AvatarUrl = "/images/default-avatar.png",
                CreatedOn = DateTime.UtcNow
            };
        }
    }

}
