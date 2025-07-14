using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DataAccess.Context;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DataAccess.Repositories.Implementations
{
    public class QueuedEmailRepository : IQueuedEmailRepository
    {
        private readonly AppDbContext _context;

        public QueuedEmailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<QueuedEmail>, int)> GetRecentSentEmailsPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.QueuedEmails
                .Where(e => e.SentAt != null)
                .OrderByDescending(e => e.SentAt);

            var totalCount = await query.CountAsync();

            var pagedEmails = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (pagedEmails, totalCount);
        }

        public async Task InsertAsync(QueuedEmail email)
        {
            _context.QueuedEmails.Add(email);
            await _context.SaveChangesAsync();
        }
    }

}
