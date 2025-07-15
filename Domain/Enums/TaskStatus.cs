namespace TaskManagementSystem.Domain.Enums
{
    public enum TaskStatus
    {
        Created,            // Created by Admin
        AssignedToUser,     // Forwarded by Manager to User
        AssignedToManager, // Assigned by Admin
        InProgress,         // User started work
        CompletionRequested,// User submitted for review
        ManagerApproved,    // Approved by manager
        Completed,      // Final approved by admin (completed)
        Reassigned,          // Reassigned (restart cycle)
        RejectedByAdmin,    // Rejected by Admin
        RejectedByManager,  // Rejected by Manager
        AdminApproved,        // Admin approved the task
    }

}
