using PrintingManagementSystem.Core;
using PrintingManagementSystem.Data;
using System;

namespace PrintingManagementSystem.Models
{
    public class LaserPrinter : Printer
    {
        public LaserPrinter(string name, LogManager logManager)
            : base(name, queueCapacity: 10, logManager) { }

        public override void ProcessJob()
        {
            if (JobQueue.IsEmpty)
            {
                _logManager.LogError(Name, PrinterError.None);
                return;
            }

            Status = PrinterStatus.Busy;
            PrintJob job = JobQueue.ProcessNextJob();

            Console.WriteLine($"[Laser Printer: {Name}] Printing {job.DocumentName} ({job.Pages} pages)");

            // Faster processing time (0.5 sec per page)
            TimeSpan processingTime = TimeSpan.FromMilliseconds(job.EstimatedTime * 0.5);
            System.Threading.Thread.Sleep((int)processingTime.TotalMilliseconds);

            if (new Random().Next(1, 10) <= 2) // 20% chance of error
            {
                HandleError(PrinterErrorManager.GetRandomError());
            }
            else
            {
                _logManager.LogJob(job, Name, processingTime);
                Status = PrinterStatus.Ready;
            }
        }
    }
}
