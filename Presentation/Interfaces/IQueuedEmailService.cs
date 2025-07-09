using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Shared;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface IQueuedEmailService
    {
        Task EnqueueEmailAsync(QueuedEmail email);
        Task<PagedResult<QueuedEmailDto>> GetSentEmailsAsync(int pageNumber, int pageSize);
    }
}
