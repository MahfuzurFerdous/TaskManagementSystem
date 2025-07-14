namespace TaskManagementSystem.Application.DTOs
{
    public class TaskCardListDtoModel
    {
        public IList<TaskCardDto>? TaskCards { get; set; }
        public TaskCardSearchDto? SearchModel { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

}
