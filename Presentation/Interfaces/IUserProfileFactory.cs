using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface IUserProfileFactory
    {
        UserProfile Create(string userId, string fullName);
    }

}
