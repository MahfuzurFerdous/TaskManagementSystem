namespace TaskManagementSystem.Application.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, string? ErrorMessage)> LoginAsync(string email, string password);
        Task<(bool Success, string? ErrorMessage)> RegisterAsync(string fullName, string email, string password);
        Task LogoutAsync();
    }

}
