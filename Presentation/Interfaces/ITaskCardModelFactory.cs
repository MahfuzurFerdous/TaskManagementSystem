using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface ITaskCardModelFactory
    {
        //List<UserTaskCardDto> PrepareUserTaskCardDtos(IEnumerable<TaskCard> taskCards);
        Task<CreateTaskCardDto> PrepareCreateTaskCardViewModelAsync();
        Task<List<SelectListItem>> GetUserSelectListAsync();
        Task<EditTaskCardDto> PrepareEditTaskCardViewModelAsync(TaskCard taskCard);
        Task<EditTaskCardDto> GetAvailableUserSelectListAsync(EditTaskCardDto dto);
        //Task<TaskCardListViewDto> PrepareListModelAsync(int pageIndex, int pageSize);
        Task<TaskCardListViewDto> PrepareUserListModelAsync(TaskCardListViewDto dto);
        Task<AssignToUserViewModelDto> PrepareAssignToUserModelAsync(TaskCard task, ClaimsPrincipal user);
        Task<List<SelectListItem>> GetManagersSelectListAsync();
        Task<TaskCardDto> PrepareDetailsAsync(TaskCard taskCard);
        Task<TaskCardDto> PrepareResetTaskDtoAsync(TaskCard task);
        Task<AssignToUserViewModelDto> PrepareReassignTaskAsync(int taskId, string newAssignedUserName, ClaimsPrincipal user);
        Task<AssignToUserViewModelDto> PrepareAssignToUserModelAsync(int taskId, ClaimsPrincipal user);
        Task<TaskCardListDtoModel> PrepareListAsync(int pageNumber, int pageSize);
        Task<TaskCard> PrepareTaskCardForRejectionAsync(int taskId, string level, string rejectedBy, string reason);



    }
}
