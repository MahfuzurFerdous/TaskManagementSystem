using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmailController : Controller
    {
        private readonly IQueuedEmailService _emailService;
        private readonly IMapper _mapper;

        public EmailController(IQueuedEmailService emailService, IMapper mapper)
        {
            _emailService = emailService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SentEmails(int pageNumber = 1, int pageSize = 10)
        {
            var pagedResult = await _emailService.GetSentEmailsAsync(pageNumber, pageSize);

            var model = new QueuedEmailListModel
            {
                Emails = _mapper.Map<List<QueuedEmailDto>>(pagedResult.Items),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = pagedResult.TotalCount
            };

            return View(model);
        }

    }

}
