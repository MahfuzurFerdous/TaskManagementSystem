namespace TaskManagementSystem.Application.DTOs
{
    public class TaskCardListViewDto
    {
        public List<UserTaskCardDto> TaskCards { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
