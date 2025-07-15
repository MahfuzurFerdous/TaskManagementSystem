namespace TaskManagementSystem.Web.Models
{
    public class TaskCardSearchModel
    {
        public string? Title { get; set; }
        public string? AssignedToUserName { get; set; }
        public TaskStatus? Status { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
