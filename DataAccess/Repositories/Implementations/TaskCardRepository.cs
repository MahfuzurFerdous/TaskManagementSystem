using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DataAccess.Context;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DataAccess.Repositories.Implementations
{
    public class TaskCardRepository : ITaskCardRepository
    {
        private readonly AppDbContext _context;
        public TaskCardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskCard>> GetAllAsync() => await _context.TaskCards.ToListAsync();

        public async Task<TaskCard?> GetByIdAsync(int id)
        {
            return await _context.TaskCards
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(TaskCard taskCard)
        {
            await _context.TaskCards.AddAsync(taskCard);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskCard taskCard)
        {
            _context.TaskCards.Update(taskCard);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.TaskCards.FindAsync(id);
            if (task != null)
            {
                _context.TaskCards.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskCard>> GetByAssignedUserIdAsync(string userName)
        {
            return await _context.TaskCards
                .Where(tc => tc.AssignedToUserName == userName)
                .ToListAsync();
        }

        public async Task<List<TaskCard>> GetByAssignedUserNameAsync(string userName)
        {
            return await _context.TaskCards
                .Where(tc => tc.AssignedToUserName == userName)
                .ToListAsync();
        }

        public async Task<(IEnumerable<TaskCard> Items, int TotalCount)> GetPagedByUserAsync(string username, int pageIndex, int pageSize)
        {
            var query = _context.TaskCards
                .Where(tc => tc.AssignedToUserName == username)
                .OrderByDescending(tc => tc.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task InsertAsync(TaskStandupLog log)
        {
            _context.TaskStandupLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<TaskStandupLog>> GetByTaskCardIdAsync(int taskCardId)
        {
            return await _context.TaskStandupLogs
                .Where(l => l.TaskCardId == taskCardId)
                .OrderByDescending(l => l.SubmittedAt)
                .ToListAsync();
        }

        public IQueryable<TaskCard> GetAllTaskCards()
        {
            return _context.TaskCards
                .AsNoTracking();
        }

        public IQueryable<TaskStandupLog> GetByTaskCardId(int taskCardId)
        {
            return _context.TaskStandupLogs
                .Where(log => log.TaskCardId == taskCardId)
                .AsNoTracking();
        }

        public async Task<TaskCard?> GetByTitleAndCreatorAsync(string title, string createdByUserName)
        {
            return await _context.TaskCards
                .FirstOrDefaultAsync(tc => tc.Title == title && tc.AssignedByUserName == createdByUserName);
        }

        public async Task<TaskCard?> GetByTitleAndAssignedAsync(string title, string assignedToUserName)
        {
            return await _context.TaskCards
                .FirstOrDefaultAsync(tc => tc.Title == title && tc.AssignedToUserName == assignedToUserName);
        }
    }

}
