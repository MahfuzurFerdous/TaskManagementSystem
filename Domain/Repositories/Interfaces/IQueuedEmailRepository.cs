using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DataAccess.Repositories.Interfaces
{
    public interface IQueuedEmailRepository
    {
        Task<(List<QueuedEmail>, int)> GetRecentSentEmailsPagedAsync(int pageNumber, int pageSize);
        Task InsertAsync(QueuedEmail email);
    }


}
