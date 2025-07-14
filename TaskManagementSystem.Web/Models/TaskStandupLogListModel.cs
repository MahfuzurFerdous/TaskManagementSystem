namespace TaskManagementSystem.Web.Models
{
    public class TaskStandupLogListModel
    {
        public List<TaskStandupLogViewModel>? Logs { get; set; }
        public int TaskId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

}
