using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Application.DTOs
{
    public class UpdateTaskCardDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AssignedToUserName { get; set; }
        public DateTime? DueDate { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
    }

}
