using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DataAccess.Context;
using TaskManagementSystem.DataAccess.Repositories.Implementations;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.DataAccess.Repositories.Interfaces
{
    public interface IQueuedEmailRepository
    {
        Task<(List<QueuedEmail>, int)> GetRecentSentEmailsPagedAsync(int pageNumber, int pageSize);
        Task InsertAsync(QueuedEmail email);
    }


}
