using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface ITaskCardModelFactory
    {
        Task<CreateTaskCardDto> PrepareCreateTaskCardViewModelAsync();
        Task<List<SelectListItem>> GetUserSelectListAsync();
        //Task<EditTaskCardDto> PrepareEditTaskCardViewModelAsync(TaskCard taskCard);
        Task<EditTaskCardDto> GetAvailableUserSelectListAsync(EditTaskCardDto dto);
        Task<TaskCardListViewDto> PrepareUserListModelAsync(TaskCardListViewDto dto);
        Task<AssignToUserViewModelDto> PrepareAssignToUserModelAsync(TaskCard task, ClaimsPrincipal user);
        Task<List<SelectListItem>> GetManagersSelectListAsync();
        Task<TaskCardDto> PrepareDetailsAsync(TaskCard taskCard);
        Task<TaskCardDto> PrepareResetTaskDtoAsync(TaskCard task);
        Task<AssignToUserViewModelDto> PrepareReassignTaskAsync(int taskId, string newAssignedUserName, ClaimsPrincipal user);
        Task<AssignToUserViewModelDto> PrepareAssignToUserModelAsync(int taskId, ClaimsPrincipal user);
        Task<TaskCardListDtoModel> PrepareListAsync(TaskCardSearchDto searchModel);
        Task<TaskCard> PrepareTaskCardForRejectionAsync(int taskId, string level, string rejectedBy, string reason);
        Task<TaskCardDto> PrepareTaskCardViewModelAsync(int id, int standupPage = 1, int standupPageSize = 5);
        Task UpdateTaskCardAsync(UpdateTaskCardDto dto);
        Task<TaskCardDto?> PrepareTaskCardDtoAsync(int id);
        //Task<List<SelectListItem>> PrepareTaskStatusListAsync();
        //Task<List<SelectListItem>> PrepareAssignedUserListAsync(ApplicationUser currentUser);
        Task<EditTaskCardDto> PrepareEditTaskCardViewModelAsync(TaskCard taskCard, ApplicationUser currentUser);



    }
}
