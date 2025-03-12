namespace PrintingManagementSystem.Models
{
    public enum JobPriority
    {
        Urgent = 3, // Highest priority, processed first
        Standard = 2, // Default priority
        Low = 1 // Lowest priority, may be delayed or dropped in overflow
    }
}

