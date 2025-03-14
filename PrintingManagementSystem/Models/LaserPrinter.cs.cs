using PrintingManagementSystem.Core;
using PrintingManagementSystem.Data;
using System;
using System.Threading.Tasks;

namespace PrintingManagementSystem.Models
{
    public class LaserPrinter : Printer
    {
        public LaserPrinter(string name, LogManager logManager)
            : base(name, queueCapacity: 10, logManager) { }

        public override async Task ProcessJobAsync()
        {
            if (PrinterQueue.IsEmpty)
            {
                return;
            }

            Status = PrinterStatus.Busy; // Set status to busy while processing job
            PrintJob job = PrinterQueue.ProcessNextJob(); // Get next job from queue (Dequeue)
            _logManager.LogStartPrinting(this.Name); // Log start of printing job

            // Faster processing time (0.5 sec per page)
            TimeSpan processingTime = TimeSpan.FromMilliseconds(job.EstimatedTime * 0.5);
            await Task.Delay((int)processingTime.TotalMilliseconds); // Simulate print time

            if (new Random().Next(1, 10) <= 2) // 20% chance of error
            {
                await HandleErrorAsync(PrinterErrorManager.GetRandomError());
            }
            else
            {
                _logManager.LogJob(job, Name, processingTime);
                Status = PrinterStatus.Ready; // Set status to ready
            }
        }
    }
}