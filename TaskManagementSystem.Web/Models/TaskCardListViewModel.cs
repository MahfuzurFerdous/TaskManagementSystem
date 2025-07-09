namespace TaskManagementSystem.Web.Models
{
    public class TaskCardListViewModel
    {
        public List<UserTaskCardViewModel> TaskCards { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

}
