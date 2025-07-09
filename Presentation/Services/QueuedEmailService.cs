using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Shared;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Services
{
    public class QueuedEmailService : IQueuedEmailService
    {
        private readonly IQueuedEmailRepository _emailRepository;

        public QueuedEmailService(IQueuedEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public async Task EnqueueEmailAsync(QueuedEmail email)
        {
            await _emailRepository.InsertAsync(email);
        }

        public async Task<PagedResult<QueuedEmailDto>> GetSentEmailsAsync(int pageNumber, int pageSize)
        {
            var (emails, totalCount) = await _emailRepository.GetRecentSentEmailsPagedAsync(pageNumber, pageSize);

            var items = emails.Select(e => new QueuedEmailDto
            {
                Id = e.Id,
                To = e.To,
                Subject = e.Subject,
                CreatedAt = e.CreatedAt,
                SentAt = e.SentAt
            }).ToList();

            return new PagedResult<QueuedEmailDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

    }

}
