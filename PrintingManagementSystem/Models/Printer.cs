using PrintingManagementSystem.Core;
using PrintingManagementSystem.Data;
using PrintingManagementSystem.Services;
using System;
using System.Threading.Tasks;

namespace PrintingManagementSystem.Models
{
    public abstract class Printer : IPrinter
    {
        public string Name { get; }
        public PrinterStatus Status { get; set; }
        public PrinterQueue PrinterQueue { get; }
        private readonly Random _random;
        protected readonly LogManager _logManager;

        public Printer(string name, int queueCapacity, LogManager logManager)
        {
            Name = name;
            Status = PrinterStatus.Ready;
            PrinterQueue = new PrinterQueue(queueCapacity);
            _random = new Random();
            _logManager = logManager;
        }

        public async Task AssignJobAsync(PrintJob job)
        {
            PrinterQueue.AssignJob(job);
        }

        public virtual async Task ProcessJobAsync()
        {
            if (PrinterQueue.IsEmpty)
            {
                return;
            }

            Status = PrinterStatus.Busy; // Set status to busy while processing job
            PrintJob job = PrinterQueue.ProcessNextJob(); // Get next job from queue (Dequeue)
            _logManager.LogStartPrinting(Name); // Log start of printing job
            TimeSpan processingTime = TimeSpan.FromMilliseconds(job.EstimatedTime);
            await Task.Delay(job.EstimatedTime); // Simulate print time

            if (_random.Next(1, 10) <= 2) // 20% chance of error occurring
            {
                await HandleErrorAsync(PrinterErrorManager.GetRandomError());
                Status = PrinterStatus.Error;
            }
            else
            {
                _logManager.LogJob(job, Name, processingTime);
                Status = PrinterStatus.Ready;
            }
        }

        public virtual async Task HandleErrorAsync(PrinterError error)
        {
            Status = PrinterStatus.Error;
            _logManager.LogError(Name, error);
            await Task.Delay(5000); // Simulate recovery time
            Status = PrinterStatus.Ready;
        }
    }
}