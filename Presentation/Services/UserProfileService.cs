using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;

namespace TaskManagementSystem.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _profileRepo;
        private readonly IUserProfileFactory _profileFactory;

        public UserProfileService(IUserProfileRepository profileRepo, IUserProfileFactory profileFactory)
        {
            _profileRepo = profileRepo;
            _profileFactory = profileFactory;
        }

        public async Task CreateProfileForUserAsync(string userId, string fullName)
        {
            var profile = _profileFactory.Create(userId, fullName);
            await _profileRepo.CreateAsync(profile);
        }

        public async Task UpdateProfileAsync(string userId, UserProfileDto dto, string webRootPath)
        {
            var profile = await _profileRepo.GetByUserIdAsync(userId);
            if (profile == null)
                return;

            profile.Bio = dto.Bio;

            if (dto.AvatarFile != null && dto.AvatarFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(dto.AvatarFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    throw new InvalidOperationException("Unsupported file type.");

                var fileName = $"{Guid.NewGuid()}{extension}";
                var relativePath = Path.Combine("images", "profiles", fileName);
                var fullPath = Path.Combine(webRootPath, relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await dto.AvatarFile.CopyToAsync(stream);

                profile.AvatarUrl = "/" + relativePath.Replace("\\", "/");
            }

            await _profileRepo.UpdateAsync(profile);

        }

        public async Task<UserProfileDto> GetProfileForEditAsync(string userId)
        {
            var profile = await _profileRepo.GetByUserIdAsync(userId);
            if (profile == null)
                throw new InvalidOperationException($"Profile for userId '{userId}' not found.");

            return new UserProfileDto
            {
                FullName = profile.FullName,
                Bio = profile.Bio,
                AvatarUrl = profile.AvatarUrl
            };
        }
    }

}
