using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Factories
{
    public class TaskCardModelFactory : ITaskCardModelFactory
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskCardService _taskCardService;

        public TaskCardModelFactory(IMapper mapper, UserManager<ApplicationUser> userManager, ITaskCardService taskCardService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _taskCardService = taskCardService;
        }

        public async Task<CreateTaskCardDto> PrepareCreateTaskCardViewModelAsync()
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");

            var viewModel = new CreateTaskCardDto
            {
                AvailableUsers = managers.Select(u => new SelectListItem
                {
                    Text = u.UserName,
                    Value = u.UserName
                }).ToList()
            };


            return viewModel;
        }

        public async Task<List<SelectListItem>> GetUserSelectListAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            return users.Select(u => new SelectListItem
            {
                Value = u.UserName,
                Text = u.UserName
            }).ToList();
        }

        public async Task<EditTaskCardDto> PrepareEditTaskCardViewModelAsync(TaskCard taskCard)
        {
            var model = _mapper.Map<EditTaskCardDto>(taskCard);

            var users = await _userManager.Users.ToListAsync();

            model.AvailableUsers = users.Select(u => new SelectListItem
            {
                Text = u.UserName,
                Value = u.UserName
            }).ToList();

            return model;
        }

        public async Task<EditTaskCardDto> GetAvailableUserSelectListAsync(EditTaskCardDto dto)
        {
            dto ??= new EditTaskCardDto();

            var users = await _userManager.Users.ToListAsync();

            dto.AvailableUsers = users.Select(u => new SelectListItem
            {
                Text = u.UserName,
                Value = u.UserName
            }).ToList();

            return dto;
        }


        public Task<TaskCardListViewDto> PrepareUserListModelAsync(TaskCardListViewDto dto)
        {
            var viewModel = new TaskCardListViewDto
            {
                TaskCards = _mapper.Map<List<UserTaskCardDto>>(dto.TaskCards),
                TotalCount = dto.TotalCount,
                PageIndex = dto.PageIndex,
                PageSize = dto.PageSize
            };
            return Task.FromResult(viewModel);
        }

        public async Task<AssignToUserViewModelDto> PrepareAssignToUserModelAsync(TaskCard task, ClaimsPrincipal user)
        {
            ArgumentNullException.ThrowIfNull(task, nameof(task));
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            string targetRole = user.IsInRole("Admin") ? "Manager" : "User";

            var usersInRole = await _userManager.GetUsersInRoleAsync(targetRole);

            return new AssignToUserViewModelDto
            {
                TaskCardId = task.Id,
                AssignedToUserName = task.AssignedToUserName ?? string.Empty, 
                AvailableUsers = usersInRole.Select(u => new SelectListItem
                {
                    Text = u.UserName,
                    Value = u.UserName,
                    Selected = u.UserName == task.AssignedToUserName
                }).ToList()
            };
        }

        public async Task<AssignToUserViewModelDto> PrepareAssignToUserModelAsync(int taskId, ClaimsPrincipal user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var task = await _taskCardService.GetByIdAsync(taskId); 

            string targetRole = user.IsInRole("Admin") ? "Manager" : "User";
            var usersInRole = await _userManager.GetUsersInRoleAsync(targetRole);

            return new AssignToUserViewModelDto
            {
                TaskCardId = task.Id,
                AssignedToUserName = task.AssignedToUserName ?? string.Empty,
                AvailableUsers = usersInRole.Select(u => new SelectListItem
                {
                    Text = u.UserName,
                    Value = u.UserName,
                    Selected = u.UserName == task.AssignedToUserName
                }).ToList()
            };
        }

        public async Task<AssignToUserViewModelDto> PrepareReassignTaskAsync(int taskId, string newAssignedUserName, ClaimsPrincipal user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var currentUser = user.Identity?.Name ?? throw new ArgumentException("User identity is not valid.");

            await _taskCardService.ReassignTaskAsync(taskId, newAssignedUserName, currentUser);

            return await PrepareAssignToUserModelAsync(taskId, user);
        }

        public async Task<List<SelectListItem>> GetManagersSelectListAsync()
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");

            return managers.Select(manager => new SelectListItem
            {
                Text = manager.UserName,
                Value = manager.UserName
            }).ToList();
        }

        public Task<TaskCardDto> PrepareResetTaskDtoAsync(TaskCard task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            task.IsManagerApproved = false;
            task.ManagerApprovedAt = null;
            task.ManagerApprovedBy = null;
            task.IsAdminApproved = false;
            task.AdminApprovedAt = null;
            task.AdminApprovedBy = null;
            task.CompletionRequestedAt = null;
            task.IsCompleted = false;
            task.IsRequestedForCompletion = false;
            task.Status = Domain.Enums.TaskStatus.Reassigned;
            task.ReassignedAt = DateTime.UtcNow.AddHours(6);

            var dto = _mapper.Map<TaskCardDto>(task);
            return Task.FromResult(dto);
        }


        public async Task<TaskCardDto> PrepareDetailsAsync(TaskCard taskCard)
        {
            ArgumentNullException.ThrowIfNull(taskCard);

            var dto = _mapper.Map<TaskCardDto>(taskCard);

            var users = await _userManager.Users.ToListAsync();

            dto.AvailableUsers = users.Select(u => new SelectListItem
            {
                Text = u.UserName,
                Value = u.UserName
            }).ToList();

            return dto;
        }

        public async Task<TaskCardListDtoModel> PrepareListAsync(int pageNumber, int pageSize)
        {
            var pagedDto = await _taskCardService.GetAllAsync(pageNumber, pageSize);

            return new TaskCardListDtoModel
            {
                TaskCards = pagedDto.Items.ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = pagedDto.TotalCount
            };
        }

        public async Task<TaskCard> PrepareTaskCardForRejectionAsync(int taskId, string level, string rejectedBy, string reason)
        {
            var task = await _taskCardService.GetByIdAsync(taskId);
            if (task == null)
                throw new Exception("Task not found");

            if (level == "manager")
            {
                task.ManagerRejectionReason = reason;
                task.Status = Domain.Enums.TaskStatus.RejectedByManager;

                await _taskCardService.UpdateTaskStatusAsync(taskId, task.Status, rejectedBy);
            }
            else if (level == "admin")
            {
                task.AdminRejectionReason = reason;
                task.Status = Domain.Enums.TaskStatus.RejectedByAdmin;

                await _taskCardService.UpdateTaskStatusAsync(taskId, task.Status, rejectedBy);
            }
            else
            {
                throw new Exception("Invalid rejection level");
            }

            return task;
        }


    }

}


