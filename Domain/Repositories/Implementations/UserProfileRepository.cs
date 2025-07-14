using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DataAccess.Context;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DataAccess.Repositories.Implementations
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext _context;

        public UserProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(UserProfile profile)
        {
            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public async Task<UserProfile> GetByUserIdAsync(string userId)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (userProfile == null)
            {
                throw new InvalidOperationException($"UserProfile with UserId '{userId}' not found.");
            }
            return userProfile;
        }

        public async Task UpdateAsync(UserProfile profile)
        {
            _context.UserProfiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserProfile>> GetAllProfilesAsync()
        {
            return await _context.UserProfiles
                .Select(u => new UserProfile
                {
                    FullName = u.FullName,
                    AvatarUrl = u.AvatarUrl,
                    CreatedOn = u.CreatedOn
                })
                .ToListAsync();
        }

    }

}
