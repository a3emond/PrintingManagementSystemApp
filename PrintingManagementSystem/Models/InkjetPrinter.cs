using PrintingManagementSystem.Core;
using PrintingManagementSystem.Data;
using System;
using System.Threading.Tasks;

namespace PrintingManagementSystem.Models
{
    public class InkjetPrinter : Printer
    {
        public InkjetPrinter(string name, LogManager logManager)
            : base(name, queueCapacity: 5, logManager) { }

        public override async Task ProcessJobAsync()
        {
            if (PrinterQueue.IsEmpty)
            {
                return;
            }

            Status = PrinterStatus.Busy;
            PrintJob job = PrinterQueue.ProcessNextJob();
            _logManager.LogStartPrinting(Name);

            // Slower processing time (1.5 sec per page)
            TimeSpan processingTime = TimeSpan.FromMilliseconds(job.EstimatedTime * 1.5);
            await Task.Delay((int)processingTime.TotalMilliseconds);

            if (new Random().Next(1, 10) <= 2) // 20% chance of error
            {
                await HandleErrorAsync(PrinterErrorManager.GetRandomError());
            }
            else
            {
                _logManager.LogJob(job, Name, processingTime);
                Status = PrinterStatus.Ready;
            }
        }
    }
}