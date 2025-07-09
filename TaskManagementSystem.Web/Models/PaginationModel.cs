namespace TaskManagementSystem.Web.Models
{
    public class PaginationModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public Dictionary<string, string>? RouteValues { get; set; } = new();
    }

}
