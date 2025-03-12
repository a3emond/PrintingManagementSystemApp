namespace PrintingManagementSystem.Models
{
    public enum PrinterStatus
    {
        Ready,   // (Green) Printer is idle and can take new jobs
        Busy,    // (Yellow) Printer is currently processing a job
        Error    // (Red) Printer has encountered an issue
    }
}
