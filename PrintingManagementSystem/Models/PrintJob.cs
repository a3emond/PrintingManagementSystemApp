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
        public int EstimatedTime { get; } // Time in milliseconds

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
            int baseTime = Pages * 1000; // 1 second per page
            if (IsColor) baseTime += 500 * Pages; // Adds 0.5 sec per page if color
            return baseTime;
        }

        public int CompareTo(PrintJob other)
        {
            if (other == null) return 1;
            return other.Priority.CompareTo(this.Priority); // Higher priority jobs first
        }

        public override string ToString()
        {
            return $"[{Priority}] {DocumentName} - {Pages} pages ({(IsColor ? "Color" : "B&W")})";
        }
    }
}
