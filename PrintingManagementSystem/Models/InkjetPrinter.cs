using PrintingManagementSystem.Core;
using PrintingManagementSystem.Data;
using System;

namespace PrintingManagementSystem.Models
{
    public class InkjetPrinter : Printer
    {
        public InkjetPrinter(string name, LogManager logManager)
            : base(name, queueCapacity: 5, logManager) { }

        public override void ProcessJob()
        {
            if (JobQueue.IsEmpty)
            {
                _logManager.LogError(Name, PrinterError.None);
                return;
            }

            Status = PrinterStatus.Busy;
            PrintJob job = JobQueue.ProcessNextJob();

            Console.WriteLine($"[Inkjet Printer: {Name}] Printing {job.DocumentName} ({job.Pages} pages)");

            // Slower processing time (1.5 sec per page)
            TimeSpan processingTime = TimeSpan.FromMilliseconds(job.EstimatedTime * 1.5);
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
