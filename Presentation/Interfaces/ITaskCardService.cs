using System.Security.Claims;
using TaskManagementSystem.Application.DTOs;

using TaskManagementSystem.Application.Shared;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface ITaskCardService
    {
        Task CreateAsync(CreateTaskCardDto dto, string assignedByUserName);
        Task<PagedResult<TaskCardDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<bool> RequestCompletionAsync(int taskCardId, string currentUser);
        Task<List<UserTaskCardDto>> GetTaskCardsByUserNameAsync(string userName);
        Task<TaskCardListViewDto> GetPagedTaskCardsByUserNameAsync(string username, int pageIndex, int pageSize);
        Task AssignToUserByUserNameAsync(AssignToUserViewModelDto dto, ClaimsPrincipal user);
        Task ReassignTaskAsync(int taskId, string newAssignedUserName, string currentUser);
        Task UpdateTaskStatusAsync(int taskId, Domain.Enums.TaskStatus newStatus, string currentUser);
        Task<TaskStandupLogListDto> GetStandupLogsAsync(int taskCardId, int pageNumber, int pageSize);
        Task SubmitStandupAsync(int taskCardId, string note, string currentUser);
        Task<TaskCard> GetByIdAsync(int taskId);
        Task UpdateTaskAsync(TaskCard task);
        Task DeleteTaskAsync(int id);
        Task<TaskCard?> GetByTitleAndCreatorAsync(string title, string createdByUserName);
        Task<TaskCard?> GetByTitleAndAssignedAsync(string title, string createdByUserName);
    }

}
