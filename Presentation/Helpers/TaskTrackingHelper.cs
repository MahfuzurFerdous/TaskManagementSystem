using System.Threading.Tasks;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Web.Helpers
{
    public static class TaskTrackingHelper
    {
        public static void UpdateTrackingFields(TaskCard card, Domain.Enums.TaskStatus eventType, string actor)
        {
            card.LastModifiedBy = actor;
            card.LastUpdatedAt = DateTime.UtcNow;

            switch (eventType)
            {
                case Domain.Enums.TaskStatus.AssignedToUser:
                    card.AssignedAt = DateTime.UtcNow;
                    break;                
                
                case Domain.Enums.TaskStatus.AssignedToManager:
                    card.AssignedAt = DateTime.UtcNow;
                    card.IsManagerApproved = false;
                    card.IsAdminApproved = false;
                    card.IsRequestedForCompletion = false;
                    card.IsStarted = false;
                    card.IsCompleted = false;
                    card.AssignedByUserName = actor;
                    card.AdminApprovedAt = null;
                    card.ManagerApprovedAt = null;
                    card.CompletedAt = null;
                    card.ReassignedAt = null;
                    card.ManagerApprovedBy = null;
                    card.AdminApprovedBy = null;
                    card.ManagerRejectedBy = null;
                    card.ManagerRejectedAt = null;
                    card.AdminRejectedBy = null;
                    card.AdminRejectedAt = null;
                    card.ManagerRejectionReason = null;
                    card.AdminRejectionReason = null;
                    card.ManagerRejectionReason = null;
                    card.StartedAt = null;
                    card.CompletionRequestedAt = null;
                    break;

                case Domain.Enums.TaskStatus.InProgress:
                    card.StartedAt = DateTime.UtcNow;
                    card.IsStarted = true;
                    break;

                case Domain.Enums.TaskStatus.CompletionRequested:
                    card.CompletionRequestedAt = DateTime.UtcNow;
                    card.IsRequestedForCompletion = true;
                    card.IsRejectedByAdmin = false;
                    card.IsRejectedByManager = false;
                    break;

                case Domain.Enums.TaskStatus.ManagerApproved:
                    card.ManagerApprovedAt = DateTime.UtcNow;
                    break;

                case Domain.Enums.TaskStatus.Completed:
                    card.CompletedAt = DateTime.UtcNow;
                    break;

                case Domain.Enums.TaskStatus.Reassigned:
                    card.ReassignedAt = DateTime.UtcNow;
                    card.AssignedAt = null;
                    card.AdminApprovedAt = null;
                    card.AdminApprovedAt = null;
                    card.CompletedAt = null;
                    card.IsCompleted = false;
                    card.IsStarted = false;
                    card.IsAdminApproved = false;
                    card.IsManagerApproved = false;
                    card.IsRequestedForCompletion = false;
                    card.ManagerApprovedAt = null;
                    card.ManagerApprovedBy = null;
                    card.AssignedByUserName = actor;
                    break;

                case Domain.Enums.TaskStatus.RejectedByManager:
                    card.ManagerRejectedBy = actor;
                    card.ManagerRejectedAt = DateTime.UtcNow;
                    card.IsRejectedByManager = true;
                    card.IsRequestedForCompletion = false;
                    card.IsStarted = false;
                    break;                
                
                case Domain.Enums.TaskStatus.RejectedByAdmin:
                    card.AdminRejectedBy = actor;
                    card.AdminRejectedAt = DateTime.UtcNow;
                    card.IsRejectedByAdmin = true;
                    card.IsRequestedForCompletion = false;
                    card.IsManagerApproved = false;
                    card.IsStarted = false;
                    break;
            }
        }
    }

}
