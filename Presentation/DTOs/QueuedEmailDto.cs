using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Application.DTOs
{
    public class QueuedEmailDto
    {
        public int Id { get; set; }
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string From { get; set; } = "noreply@example.com";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Sent { get; set; } = false;
        public DateTime? SentAt { get; set; } = null;
    }
}
