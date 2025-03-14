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
            if (PrinterQueue.IsEmpty)
            {
                return;
            }
            // Set status to busy while processing job
            Status = PrinterStatus.Busy;
            // Get next job from queue (Dequeue)
            PrintJob job = PrinterQueue.ProcessNextJob();
            // Log start of printing job
            _logManager.LogStartPrinting(this.Name);
            // Faster processing time (0.5 sec per page)
            TimeSpan processingTime = TimeSpan.FromMilliseconds(job.EstimatedTime * 0.5);
            // Simulate print time
            System.Threading.Thread.Sleep((int)processingTime.TotalMilliseconds);

            if (new Random().Next(1, 10) <= 2) // 20% chance of error
            {
                HandleError(PrinterErrorManager.GetRandomError());
            }
            else
            {
                _logManager.LogJob(job, Name, processingTime);
                // Set status to ready
                Status = PrinterStatus.Ready;
            }
        }
    }
}
