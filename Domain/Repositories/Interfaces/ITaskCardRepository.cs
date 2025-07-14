using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DataAccess.Repositories.Interfaces
{
    public interface ITaskCardRepository
    {
        Task<IEnumerable<TaskCard>> GetAllAsync();
        Task<TaskCard?> GetByIdAsync(int id);
        Task AddAsync(TaskCard taskCard);
        Task UpdateAsync(TaskCard taskCard);
        Task DeleteAsync(int id);
        Task<IEnumerable<TaskCard>> GetByAssignedUserIdAsync(string userId);
        Task<List<TaskCard>> GetByAssignedUserNameAsync(string userName);
        Task<(IEnumerable<TaskCard> Items, int TotalCount)> GetPagedByUserAsync(string username, int pageIndex, int pageSize);
        Task InsertAsync(TaskStandupLog log);
        Task<IList<TaskStandupLog>> GetByTaskCardIdAsync(int taskCardId);
        IQueryable<TaskCard> GetAllTaskCards();
        IQueryable<TaskStandupLog> GetByTaskCardId(int taskCardId);
        Task<TaskCard?> GetByTitleAndCreatorAsync(string title, string createdByUserName);
        Task<TaskCard?> GetByTitleAndAssignedAsync(string title, string assignedToUserName);

    }
}
