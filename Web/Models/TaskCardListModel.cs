﻿using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Web.Models
{
    public class TaskCardListModel
    {
        public List<TaskCardViewModel>? TaskCards { get; set; }
        public TaskCardSearchDto SearchModel { get; set; } = new TaskCardSearchDto();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }


}
