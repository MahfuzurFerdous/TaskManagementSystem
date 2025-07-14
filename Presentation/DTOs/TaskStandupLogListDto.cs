namespace TaskManagementSystem.Application.DTOs
{
    public class TaskStandupLogListDto
    {
        public List<TaskStandupLogDto> Logs { get; set; } = new List<TaskStandupLogDto>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages =>
            PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
    }

}
