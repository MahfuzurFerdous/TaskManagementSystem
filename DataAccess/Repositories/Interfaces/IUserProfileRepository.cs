﻿using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DataAccess.Repositories.Interfaces
{
    public interface IUserProfileRepository
    {
        Task CreateAsync(UserProfile profile);
        Task<UserProfile> GetByUserIdAsync(string userId);
        Task<List<UserProfile>> GetAllProfilesAsync();
        Task UpdateAsync(UserProfile profile);
    }

}
