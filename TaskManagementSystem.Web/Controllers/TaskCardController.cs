using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    public class TaskCardController : Controller
    {
        private readonly ITaskCardService _taskCardService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskCardModelFactory _taskCardModelFactory;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ILogger<TaskCardController> _logger;

        public TaskCardController(ITaskCardService taskCardService, UserManager<ApplicationUser> userManager, ITaskCardModelFactory taskCardModelFactory, IMapper mapper, INotificationService notificationService, ILogger<TaskCardController> logger)
        {
            _taskCardService = taskCardService;
            _userManager = userManager;
            _taskCardModelFactory = taskCardModelFactory;
            _mapper = mapper;
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            var createDto = await _taskCardModelFactory.PrepareCreateTaskCardViewModelAsync();

            var model = _mapper.Map<CreateTaskCardViewModel>(createDto);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateTaskCardViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableUsers = await _taskCardModelFactory.GetManagersSelectListAsync();
                return View(model);
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
            {
                ModelState.AddModelError("", "Current user information is missing.");
                model.AvailableUsers = await _taskCardModelFactory.GetManagersSelectListAsync();
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("", "Task title cannot be null or empty.");
                model.AvailableUsers = await _taskCardModelFactory.GetManagersSelectListAsync();
                return View(model);
            }

            var dto = _mapper.Map<CreateTaskCardDto>(model);
            dto.CreatedByUserName = currentUser.UserName;
            dto.CreatedAt = DateTime.UtcNow.AddHours(6);

            try
            {
                await _taskCardService.CreateAsync(dto, currentUser.UserName);
                var createdTask = await _taskCardService.GetByTitleAndCreatorAsync(dto.Title!, currentUser.UserName);

                if (createdTask != null)
                {
                    await _notificationService.NotifyTaskCreationAsync(createdTask);
                }
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.AvailableUsers = await _taskCardModelFactory.GetManagersSelectListAsync();
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AssignTaskModal(int id)
        {
            var task = await _taskCardService.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            if (User.IsInRole("Manager") && task.AssignedToUserName != User.Identity?.Name)
                return Forbid();

            var dto = await _taskCardModelFactory.PrepareAssignToUserModelAsync(task, User);
            var model = _mapper.Map<AssignToUserViewModel>(dto);
            return PartialView("_AssignTaskPartial", model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AssignToUser(AssignToUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var task = await _taskCardService.GetByIdAsync(model.TaskCardId);
                var dto = await _taskCardModelFactory.PrepareAssignToUserModelAsync(task!, User);
                model.AvailableUsers = dto.AvailableUsers;

                return PartialView("_AssignTaskPartial", model);
            }

            try
            {
                var dto = _mapper.Map<AssignToUserViewModelDto>(model);
                dto.AssignedByUserName = User.Identity?.Name ?? "System";

                await _taskCardService.AssignToUserByUserNameAsync(dto, User);
                var task = await _taskCardService.GetByIdAsync(model.TaskCardId);
                await _notificationService.NotifyTaskAssignmentAsync(task);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var task = await _taskCardService.GetByIdAsync(model.TaskCardId);
                var dto = await _taskCardModelFactory.PrepareAssignToUserModelAsync(task!, User);
                model.AvailableUsers = dto.AvailableUsers;

                return PartialView("_AssignTaskPartial", model);
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Index(TaskCardSearchModel searchModel, int pageNumber = 1, int pageSize = 10)
        {
            searchModel.Page = pageNumber;
            searchModel.PageSize = pageSize;
            var searchDto = _mapper.Map<TaskCardSearchDto>(searchModel);

            var dtoModel = await _taskCardModelFactory.PrepareListAsync(searchDto);

            var viewModel = new TaskCardListModel
            {
                SearchModel = searchDto,
                TaskCards = _mapper.Map<List<TaskCardViewModel>>(dtoModel.TaskCards),
                PageNumber = dtoModel.PageNumber,
                PageSize = dtoModel.PageSize,
                TotalCount = dtoModel.TotalCount
            };

            return View(viewModel);
        }


        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserIndex(int page = 1)
        {
            int pageSize = 6;

            var user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.UserName))
                return NotFound("User not found or username is null.");

            var pagedDto = await _taskCardService.GetPagedTaskCardsByUserNameAsync(user.UserName, page - 1, pageSize);

            var mappedTaskCards = _mapper.Map<List<UserTaskCardViewModel>>(pagedDto.TaskCards);

            var viewModel = new TaskCardListViewModel
            {
                TaskCards = mappedTaskCards,
                TotalCount = pagedDto.TotalCount,
                PageIndex = pagedDto.PageIndex,
                PageSize = pagedDto.PageSize
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> StartTask(int id)
        {
            var task = await _taskCardService.GetByIdAsync(id);
            if (task == null)
                return Json(new { success = false, message = "Task not found." });

            var currentUser = User.Identity?.Name ?? "Unknown";

            try
            {
                await _taskCardService.UpdateTaskStatusAsync(id, Domain.Enums.TaskStatus.InProgress, currentUser);
                return Json(new { success = true, message = "Task marked as In Progress." });
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { success = false, message = "You are not assigned to this task." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RequestCompletion(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.UserName))
                return Json(new { success = false, message = "User not found or username is null." });

            var taskCard = await _taskCardService.GetByIdAsync(id);
            if (taskCard == null || taskCard.AssignedToUserName != user.UserName)
                return Json(new { success = false, message = "Task not found or not assigned to you." });

            var success = await _taskCardService.RequestCompletionAsync(id, user.UserName);

            if (!success)
                return Json(new { success = false, message = "Task is already completed or completion already requested." });

            await _notificationService.NotifyCompletionRequestAsync(user, taskCard);

            return Json(new { success = true, message = "Completion request sent to Manager and Admins." });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ApproveCompletion(int id, string level)
        {
            var task = await _taskCardService.GetByIdAsync(id);
            if (task == null)
                return Json(new { success = false, message = "Task not found." });

            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return Json(new { success = false, message = "User identity is missing." });

            if (level == "manager")
            {
                if (!User.IsInRole("Manager") || task.AssignedByUserName != userName)
                    return Json(new { success = false, message = "Unauthorized manager approval." });

                task.IsManagerApproved = true;
                task.ManagerApprovedBy = userName;
                task.ManagerApprovedAt = DateTime.UtcNow;
                task.Status = Domain.Enums.TaskStatus.ManagerApproved;

            }
            else if (level == "admin")
            {
                if (!User.IsInRole("Admin") || !task.IsManagerApproved)
                    return Json(new { success = false, message = "Admin approval requires manager approval first." });

                task.IsAdminApproved = true;
                task.AdminApprovedBy = userName;
                task.AdminApprovedAt = DateTime.UtcNow;
                task.IsCompleted = true;
                task.Status = Domain.Enums.TaskStatus.Completed;
            }

            await _taskCardService.UpdateTaskAsync(task);
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                await _notificationService.NotifyCompletionRequestAsync(user, task);
            }
            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> RejectCompletion(int id, string level, string reason)
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return Json(new { success = false, message = "User identity is missing." });

            var task = await _taskCardService.GetByIdAsync(id);
            if (task == null)
                return Json(new { success = false, message = "Task not found." });

            if (level == "manager")
            {
                if (!User.IsInRole("Manager") || task.AssignedByUserName != userName)
                    return Json(new { success = false, message = "Unauthorized manager rejection." });
            }
            else if (level == "admin")
            {
                if (!User.IsInRole("Admin") || !task.IsManagerApproved)
                    return Json(new { success = false, message = "Admin rejection requires manager approval first." });
            }

            try
            {
                var updatedTask = await _taskCardModelFactory.PrepareTaskCardForRejectionAsync(id, level, userName, reason);
                await _taskCardService.UpdateTaskAsync(updatedTask);

                if (!string.IsNullOrEmpty(task.AssignedToUserName))
                {
                    var assignedUser = await _userManager.FindByNameAsync(task.AssignedToUserName);
                    if (assignedUser != null)
                    {
                        await _notificationService.NotifyTaskRejectionAsync(assignedUser, task, reason);
                    }
                }

                if (!string.IsNullOrEmpty(task.ManagerApprovedBy))
                {
                    var approvingManager = await _userManager.FindByNameAsync(task.ManagerApprovedBy);
                    if (approvingManager != null && approvingManager.UserName != userName)
                    {
                        await _notificationService.NotifyTaskRejectionAsync(approvingManager, task, reason);
                    }
                }

                return Json(new { success = true, message = "Task completion rejected successfully." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while rejecting the task." });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ReassignTaskModal(int id)
        {
            var task = await _taskCardService.GetByIdAsync(id);
            if (task == null) return NotFound();

            var assignDto = await _taskCardModelFactory.PrepareAssignToUserModelAsync(task, User);
            var model = _mapper.Map<AssignToUserViewModel>(assignDto);

            return PartialView("_ReassignTaskPartial", model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ReassignTask(AssignToUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ReassignTaskPartial", model);
            }

            try
            {
                await _taskCardModelFactory.PrepareReassignTaskAsync(model.TaskCardId, model.AssignedToUserName, User);

                var task = await _taskCardService.GetByIdAsync(model.TaskCardId);
                var newUser = await _userManager.FindByNameAsync(model.AssignedToUserName);

                if (newUser != null && task != null)
                    await _notificationService.NotifyTaskReassignmentAsync(newUser, task);


                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_ReassignTaskPartial", model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var taskCard = await _taskCardService.GetByIdAsync(id);
            if (taskCard == null)
                return NotFound();
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return NotFound();

            var dto = await _taskCardModelFactory.PrepareEditTaskCardViewModelAsync(taskCard,currentUser);

            var viewModel = _mapper.Map<EditTaskCardViewModel>(dto);

            return PartialView("_EditTaskCardPartial", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(EditTaskCardViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Validation failed." });
            }

            var taskCardToUpdate = await _taskCardService.GetByIdAsync(viewModel.Id);
            if (taskCardToUpdate == null)
                return NotFound(new { success = false, message = "Task not found." });

            var dtoToUpdate = _mapper.Map<EditTaskCardDto>(viewModel);
            _mapper.Map(dtoToUpdate, taskCardToUpdate);

            await _taskCardService.UpdateTaskAsync(taskCardToUpdate);

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized(new { success = false, message = "User not found." });

            await _taskCardService.UpdateTaskStatusAsync(viewModel.Id, viewModel.Status, currentUser.UserName);

            return Json(new { success = true, message = "Task updated successfully.", id = viewModel.Id });

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var taskCard = await _taskCardService.GetByIdAsync(id);
            if (taskCard == null)
                return Json(new { success = false, message = "Task not found." });

            await _taskCardService.DeleteTaskAsync(taskCard.Id);
            return Json(new { success = true });
        }

        public async Task<IActionResult> Details(int id, int standupPage = 1)
        {
            var dto = await _taskCardModelFactory.PrepareTaskCardViewModelAsync(id, standupPage);
            if (dto == null)
                return NotFound();

            var viewModel = _mapper.Map<TaskCardViewModel>(dto);
            return View("Details", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, Domain.Enums.TaskStatus status)
        {
            var currentUser = User.Identity?.Name ?? "Unknown";

            await _taskCardService.UpdateTaskStatusAsync(id, status, currentUser);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitStandupLog(int taskId, string note)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.FullName))
                return Json(new { success = false, message = "User not found or full name is missing." });

            try
            {
                var fullName = user.FullName;

                await _taskCardService.SubmitStandupAsync(taskId, note, fullName);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStandupHistoryPartial(int taskId, int pageNumber = 1, int pageSize = 5)
        {
            var dto = await _taskCardService.GetStandupLogsAsync(taskId, pageNumber, pageSize);

            var model = new TaskStandupLogListModel
            {
                Logs = _mapper.Map<List<TaskStandupLogViewModel>>(dto.Logs),
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                TotalCount = dto.TotalCount,
                TaskId = taskId
            };

            return PartialView("_StandupLogPartial", model);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult GetStandupFormPartial(int taskId)
        {
            return PartialView("_SubmitStandupPartial", taskId);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateTaskCardDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _taskCardModelFactory.UpdateTaskCardAsync(dto);
                return Ok(new { message = "Task updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update task");
                return StatusCode(500, new { error = "Internal server error." });
            }
        }


    }

}
