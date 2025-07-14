using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
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

        public async Task<EditTaskCardDto> PrepareEditTaskCardViewModelAsync(TaskCard taskCard, ApplicationUser currentUser)
        {
            var model = _mapper.Map<EditTaskCardDto>(taskCard);

            model.Status = taskCard.Status;

            model.StatusList = Enum.GetValues(typeof(Domain.Enums.TaskStatus))
                .Cast<Domain.Enums.TaskStatus>()
                .Select(status => new SelectListItem
                {
                    Value = status.ToString(), 
                    Text = status.ToString(), 
                    Selected = status == taskCard.Status
                }).ToList();

            var availableUsers = new List<ApplicationUser>();

            if (await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                availableUsers = (await _userManager.GetUsersInRoleAsync("Manager")).ToList();
            }
            else if (await _userManager.IsInRoleAsync(currentUser, "Manager"))
            {
                availableUsers = (await _userManager.GetUsersInRoleAsync("User")).ToList();
            }

            model.AvailableUsers = availableUsers.Select(u => new SelectListItem
            {
                Value = u.UserName,
                Text = u.FullName ?? u.UserName,
                Selected = u.UserName == taskCard.AssignedToUserName
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

        public async Task<TaskCardListDtoModel> PrepareListAsync(TaskCardSearchDto searchModel)
        {
            var result = await _taskCardService.SearchTaskCardsAsync(searchModel);

            return new TaskCardListDtoModel
            {
                TaskCards = _mapper.Map<List<TaskCardDto>>(result.ToList()), 
                PageNumber = searchModel.Page,
                PageSize = searchModel.PageSize,
                TotalCount = result.Count 
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

        public async Task<TaskCardDto> PrepareTaskCardViewModelAsync(int id, int standupPage = 1, int standupPageSize = 5)
        {
            var task = await _taskCardService.GetByIdAsync(id);

            if (task == null)
                throw new ArgumentException("Task not found");

            var model = new TaskCardDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                AssignedToUserName = task.AssignedToUserName,
                DueDate = task.DueDate,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                StatusChanges = await _taskCardService.GetStatusChangesAsync(task),
                StandupLogList = await _taskCardService.GetStandupLogsAsync(task, standupPage, standupPageSize)
            };

            return model;
        }

        public async Task UpdateTaskCardAsync(UpdateTaskCardDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var task = await _taskCardService.GetByIdAsync(dto.Id);
            if (task == null)
                throw new ArgumentException("Task not found.");

            task.Title = dto.Title ?? string.Empty;
            task.Description = dto.Description ?? string.Empty;
            task.AssignedToUserName = dto.AssignedToUserName ?? string.Empty;
            task.DueDate = dto.DueDate;
            task.Status = dto.Status;

            await _taskCardService.UpdateTaskAsync(task);
        }

        public async Task<TaskCardDto?> PrepareTaskCardDtoAsync(int id)
        {
            var taskCardEntity = await _taskCardService.GetByIdAsync(id);
            if (taskCardEntity == null) return null;

            var dto = new TaskCardDto
            {
                Id = taskCardEntity.Id,
                Title = taskCardEntity.Title,
                Description = taskCardEntity.Description,
                AssignedToUserName = taskCardEntity.AssignedToUserName,
                DueDate = taskCardEntity.DueDate,
                CreatedAt = taskCardEntity.CreatedAt,
                Status = taskCardEntity.Status,
            };

            return dto;
        }
        //public Task<List<SelectListItem>> PrepareTaskStatusListAsync()
        //{
        //    var list = Enum.GetValues(typeof(Domain.Enums.TaskStatus))
        //        .Cast<Domain.Enums.TaskStatus>()
        //        .Select(e => new SelectListItem
        //        {
        //            Value = e.ToString(),
        //            Text = e.ToString()
        //        })
        //        .ToList();

        //    return Task.FromResult(list);
        //}

        //public async Task<List<SelectListItem>> PrepareAssignedUserListAsync(ApplicationUser currentUser)
        //{
        //    var list = new List<SelectListItem>();

        //    if (await _userManager.IsInRoleAsync(currentUser, "Admin"))
        //    {
        //        var managers = await _userManager.GetUsersInRoleAsync("Manager");
        //        list = managers.Select(m => new SelectListItem
        //        {
        //            Value = m.UserName,
        //            Text = m.FullName ?? m.UserName
        //            Selected = 
        //        }).ToList();
        //    }
        //    else if (await _userManager.IsInRoleAsync(currentUser, "Manager"))
        //    {
        //        var users = await _userManager.GetUsersInRoleAsync("User");
        //        list = users.Select(u => new SelectListItem
        //        {
        //            Value = u.UserName,
        //            Text = u.FullName ?? u.UserName
        //        }).ToList();
        //    }

        //    return list;
        //}

        public async Task<TaskCardListDtoModel> PrepareTaskCardListModelAsync(TaskCardSearchDto searchModel)
        {
            var taskCards = await _taskCardService.SearchTaskCardsAsync(searchModel);

            var model = new TaskCardListDtoModel    
            {
                SearchModel = searchModel,
                TaskCards = taskCards.Select(tc => new TaskCardDto
                {
                    Id = tc.Id,
                    Title = tc.Title,
                    AssignedToUserName = tc.AssignedToUserName,
                    Status = tc.Status,
                    DueDate = tc.DueDate
                }).ToList()
            };

            return model;
        }

    }

}


