﻿namespace TaskManagementSystem.Application.DTOs
{
    public class UserListDto
    {
        public List<UserDto> Users { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages =>
            PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
    }

}
