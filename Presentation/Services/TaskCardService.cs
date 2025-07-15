using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Shared;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Web.Helpers;

namespace TaskManagementSystem.Application.Services
{
    public class TaskCardService : ITaskCardService
    {
        private readonly ITaskCardRepository _taskCardRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public TaskCardService(ITaskCardRepository taskCardRepository, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _taskCardRepository = taskCardRepository;
            _userManager = userManager;
            _mapper = mapper;

        }

        public async Task CreateAsync(CreateTaskCardDto dto, string assignedByUserName)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(dto.Title));

            if (string.IsNullOrWhiteSpace(dto.AssignedToUserName))
                throw new ArgumentException("AssignedToUserName cannot be null or empty.", nameof(dto.AssignedToUserName));

            if (string.IsNullOrWhiteSpace(dto.CreatedByUserName))
                throw new ArgumentException("CreatedByUserName cannot be null or empty.", nameof(dto.CreatedByUserName));

            var assignedTo = await _userManager.FindByNameAsync(dto.AssignedToUserName);
            var createdBy = await _userManager.FindByNameAsync(dto.CreatedByUserName);

            if (assignedTo == null || createdBy == null)
                throw new ArgumentException("User not found.");

            var card = new TaskCard
            {
                Title = dto.Title ?? throw new ArgumentNullException(nameof(dto.Title), "Title cannot be null."),
                Description = dto.Description,
                AssignedToUserName = dto.AssignedToUserName,
                AssignedByUserName = assignedByUserName,
                CreatedAt = DateTime.UtcNow,
                DueDate = dto.DueDate,
                IsCompleted = false,
                IsRequestedForCompletion = false
            };

            await _taskCardRepository.AddAsync(card);
        }

        public async Task<PagedResult<TaskCardDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _taskCardRepository.GetAllTaskCards();
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new TaskCardDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    AssignedToUserName = c.AssignedToUserName,
                    AssignedByUserName = c.AssignedByUserName,
                    CreatedAt = c.CreatedAt,
                    DueDate = c.DueDate,
                    Status = c.Status,
                    IsCompleted = c.IsCompleted,
                    IsRequestedForCompletion = c.IsRequestedForCompletion,
                    IsManagerApproved = c.IsManagerApproved,
                    IsAdminApproved = c.IsAdminApproved,
                    IsRejectedByManager = c.IsRejectedByManager,
                    IsRejectedByAdmin = c.IsRejectedByAdmin,
                    ManagerRejectionReason = c.ManagerRejectionReason,
                    AdminRejectionReason = c.AdminRejectionReason,
                    ManagerRejectedAt = c.ManagerRejectedAt,
                    ManagerRejectedBy = c.ManagerRejectedBy,
                    AdminRejectedAt = c.AdminRejectedAt,
                    AdminRejectedBy = c.AdminRejectedBy
                })
                .ToListAsync();

            return new PagedResult<TaskCardDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<bool> RequestCompletionAsync(int taskCardId, string currentUser)
        {
            var card = await _taskCardRepository.GetByIdAsync(taskCardId);
            if (card == null || card.IsCompleted || card.IsRequestedForCompletion)
                return false;

            await UpdateTaskStatusAsync(taskCardId, Domain.Enums.TaskStatus.CompletionRequested, currentUser);

            return true;
        }

        public async Task<List<UserTaskCardDto>> GetTaskCardsByUserNameAsync(string userName)
        {
            var taskCards = await _taskCardRepository.GetByAssignedUserNameAsync(userName);
            return _mapper.Map<List<UserTaskCardDto>>(taskCards);
        }

        public async Task<TaskCardListViewDto> GetPagedTaskCardsByUserNameAsync(string username, int pageIndex, int pageSize)
        {
            pageIndex = Math.Max(0, pageIndex);
            pageSize = Math.Max(1, pageSize);

            var (items, totalCount) = await _taskCardRepository.GetPagedByUserAsync(username, pageIndex, pageSize);

            var dtos = _mapper.Map<List<UserTaskCardDto>>(items);

            return new TaskCardListViewDto
            {
                TaskCards = dtos,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task AssignToUserByUserNameAsync(AssignToUserViewModelDto dto, ClaimsPrincipal user)
        {
            var currentUser = user.Identity?.Name ?? "Unknown";

            var task = await _taskCardRepository.GetByIdAsync(dto.TaskCardId);
            if (task == null)
                throw new ArgumentException("Task not found");

            task.AssignedToUserName = dto.AssignedToUserName;
            task.AssignedByUserName = dto.AssignedByUserName;

            await _taskCardRepository.UpdateAsync(task);

            Domain.Enums.TaskStatus newStatus = user.IsInRole("Admin")
                ? Domain.Enums.TaskStatus.AssignedToManager
                : Domain.Enums.TaskStatus.AssignedToUser;

            await UpdateTaskStatusAsync(task.Id, newStatus, currentUser);
        }

        public async Task ReassignTaskAsync(int taskId, string newAssignedUserName, string currentUser)
        {
            if (string.IsNullOrWhiteSpace(currentUser))
                throw new ArgumentException("User identity is not valid.");

            var task = await _taskCardRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new ArgumentException("Task not found.");

            task.AssignedToUserName = newAssignedUserName;

            await UpdateTaskStatusAsync(task.Id, Domain.Enums.TaskStatus.Reassigned, currentUser);
        }

        public async Task<TaskCard> GetByIdAsync(int taskId)
        {
            var task = await _taskCardRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new ArgumentException("Task not found.");

            return task;
        }

        public async Task UpdateTaskStatusAsync(int taskId, Domain.Enums.TaskStatus newStatus, string currentUser)
        {
            var taskCard = await _taskCardRepository.GetByIdAsync(taskId);
            if (taskCard == null)
                throw new Exception("Task not found");

            taskCard.Status = newStatus;

            TaskTrackingHelper.UpdateTrackingFields(taskCard, newStatus, currentUser);

            await _taskCardRepository.UpdateAsync(taskCard);
        }

        public async Task SubmitStandupAsync(int taskCardId, string note, string currentUser)
        {
            var task = await _taskCardRepository.GetByIdAsync(taskCardId);
            if (task == null)
                throw new Exception("Task not found");

            var log = new TaskStandupLog
            {
                TaskCardId = taskCardId,
                SubmittedBy = currentUser,
                Note = note,
                SubmittedAt = DateTime.UtcNow
            };

            await _taskCardRepository.InsertAsync(log);
        }

        public async Task<TaskStandupLogListDto> GetStandupLogsAsync(int taskCardId, int pageNumber, int pageSize)
        {
            var query = _taskCardRepository.GetByTaskCardId(taskCardId);

            var totalCount = await query.CountAsync();

            var pagedLogs = await query
                .OrderByDescending(x => x.SubmittedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtoLogs = _mapper.Map<List<TaskStandupLogDto>>(pagedLogs);

            return new TaskStandupLogListDto
            {
                Logs = dtoLogs,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task UpdateTaskAsync(TaskCard task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            await _taskCardRepository.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _taskCardRepository.GetByIdAsync(id);
            if (task == null)
                throw new ArgumentException("Task not found.");

            await _taskCardRepository.DeleteAsync(id);
        }

        public async Task<TaskCard?> GetByTitleAndCreatorAsync(string title, string createdByUserName)
        {
            return await _taskCardRepository.GetByTitleAndCreatorAsync(title, createdByUserName);
        }

        public async Task<TaskCard?> GetByTitleAndAssignedAsync(string title, string createdByUserName)
        {
            return await _taskCardRepository.GetByTitleAndAssignedAsync(title, createdByUserName);
        }


        public Task<List<TaskStatusChangeDto>> GetStatusChangesAsync(TaskCard task)
        {
            var changes = new List<TaskStatusChangeDto>();

            if (task.AssignedAt.HasValue)
            {
                changes.Add(new TaskStatusChangeDto
                {
                    Status = Domain.Enums.TaskStatus.AssignedToUser,
                    ChangedAt = task.AssignedAt.Value,
                    ChangedBy = task.AssignedByUserName ?? "System"
                });
            }
            if (task.StartedAt.HasValue)
            {
                changes.Add(new TaskStatusChangeDto
                {
                    Status = Domain.Enums.TaskStatus.InProgress,
                    ChangedBy = task.AssignedToUserName ?? "User",
                    ChangedAt = task.StartedAt.Value,
                });
            }
            if (task.IsRequestedForCompletion && task.CompletionRequestedAt.HasValue)
            {
                changes.Add(new TaskStatusChangeDto
                {
                    Status = Domain.Enums.TaskStatus.CompletionRequested,
                    ChangedBy = task.AssignedToUserName ?? "User",
                    ChangedAt = task.CompletionRequestedAt.Value,
                });
            }
            if (task.IsManagerApproved && task.ManagerApprovedAt.HasValue)
            {
                changes.Add(new TaskStatusChangeDto
                {
                    Status = Domain.Enums.TaskStatus.ManagerApproved,
                    ChangedBy = task.ManagerApprovedBy ?? "Manager",
                    ChangedAt = task.ManagerApprovedAt.Value,
                });
            }
            if (task.IsRejectedByManager && task.ManagerRejectedAt.HasValue)
            {
                changes.Add(new TaskStatusChangeDto
                {
                    Status = Domain.Enums.TaskStatus.RejectedByManager,
                    ChangedBy = task.ManagerRejectedBy ?? "Manager",
                    ChangedAt = task.ManagerRejectedAt.Value,
                    Remarks = task.ManagerRejectionReason ?? string.Empty
                });
            }
            if (task.IsAdminApproved && task.AdminApprovedAt.HasValue)
            {
                changes.Add(new TaskStatusChangeDto
                {
                    Status = Domain.Enums.TaskStatus.AdminApproved,
                    ChangedBy = task.AdminApprovedBy ?? "Admin",
                    ChangedAt = task.AdminApprovedAt.Value,
                });
            }
            if (task.IsRejectedByAdmin && task.AdminRejectedAt.HasValue)
            {
                changes.Add(new TaskStatusChangeDto
                {
                    Status = Domain.Enums.TaskStatus.RejectedByAdmin,
                    ChangedBy = task.AdminRejectedBy ?? "Admin",
                    ChangedAt = task.AdminRejectedAt.Value,
                    Remarks = task.AdminRejectionReason ?? string.Empty
                });
            }
            if (task.CompletedAt.HasValue)
            {
                changes.Add(new TaskStatusChangeDto
                {
                    Status = Domain.Enums.TaskStatus.Completed,
                    ChangedBy = task.AdminApprovedBy ?? "Admin",
                    ChangedAt = task.CompletedAt.Value,
                });
            }
            var result = changes.OrderByDescending(c => c.ChangedAt).ToList();
            return Task.FromResult(result);
        }


        public async Task<TaskStandupLogListDto> GetStandupLogsAsync(TaskCard task, int page, int pageSize)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            var logs = task.StandupLogs?
                .OrderByDescending(log => log.SubmittedAt)
                .ToList() ?? new List<TaskStandupLog>();

            var totalCount = logs.Count;

            var pagedLogs = logs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(log => new TaskStandupLogDto
                {
                    SubmittedBy = log.SubmittedBy,
                    SubmittedAt = log.SubmittedAt,
                    Note = log.Note
                })
                .ToList();

            return await Task.FromResult(new TaskStandupLogListDto
            {
                Logs = pagedLogs,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount
            });
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await Task.FromResult(_userManager.Users.ToList());
        }

        public async Task<IList<TaskCard>> SearchTaskCardsAsync(TaskCardSearchDto model)
        {
            var query = _taskCardRepository.GetAllTaskCards();

            if (!string.IsNullOrWhiteSpace(model.Title))
                query = query.Where(t => t.Title.Contains(model.Title));

            if (!string.IsNullOrWhiteSpace(model.AssignedToUserName))
                query = query.Where(t => t.AssignedToUserName != null && t.AssignedToUserName.Contains(model.AssignedToUserName));

            if (model.Status.HasValue)
                query = query.Where(t => t.Status == model.Status.Value);

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }


    }

}
