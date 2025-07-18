﻿using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Web.Models
{
    public class QueuedEmailListModel
    {
        public List<QueuedEmailDto>? Emails { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

}
