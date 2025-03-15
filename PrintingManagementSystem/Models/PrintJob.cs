using System;

namespace PrintingManagementSystem.Models
{
    public class PrintJob : IComparable<PrintJob>
    {
        public string DocumentName { get; }
        public int Pages { get; }
        public string PaperSize { get; }
        public bool IsColor { get; }
        public JobPriority Priority { get; }
        public DateTime CreatedAt { get; }
        public int EstimatedTime { get; }
        // Time in milliseconds

        public PrintJob(string documentName, int pages, string paperSize, bool isColor, JobPriority priority)
        {
            DocumentName = documentName;
            Pages = pages;
            PaperSize = paperSize;
            IsColor = isColor;
            Priority = priority;
            CreatedAt = DateTime.Now;
            EstimatedTime = CalculateEstimatedTime();
        }

        private int CalculateEstimatedTime()
        {
            // Calculate estimated time based on pages and whether the job is color or not
            return Pages * (IsColor ? 200 : 100);
        }

        public int CompareTo(PrintJob other)
        {
            if (other == null) return 1;

            // Higher priority jobs should come first
            int priorityComparison = other.Priority.CompareTo(Priority);
            if (priorityComparison != 0)
            {
                return priorityComparison;
            }

            // If priorities are the same, compare by creation time (earlier jobs first)
            return CreatedAt.CompareTo(other.CreatedAt);
        }

        public override string ToString()
        {
            return $"{DocumentName} (Priority: {Priority})";
        }
    }
}
