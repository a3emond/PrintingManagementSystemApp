using PrintingManagementSystem.Core;
using PrintingManagementSystem.Data;
using PrintingManagementSystem.Services;

namespace PrintingManagementSystem.Models
{
    using System;
    using System.Threading;

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

        public void AssignJob(PrintJob job)
        {
            PrinterQueue.AssignJob(job);
            _logManager.LogJob(job, Name, TimeSpan.Zero);
        }

        public virtual void ProcessJob()
        {
            if (PrinterQueue.IsEmpty)
            {
                _logManager.LogError(Name, PrinterError.None);
                return;
            }

            Status = PrinterStatus.Busy; // Set status to busy while processing job
            PrintJob job = PrinterQueue.ProcessNextJob(); // Get next job from queue (Dequeue)
            _logManager.LogStartPrinting(Name); // Log start of printing job
            TimeSpan processingTime = TimeSpan.FromMilliseconds(job.EstimatedTime);
            Thread.Sleep(job.EstimatedTime); // Simulate print time

            if (_random.Next(1, 10) <= 2) // 20% chance of error occurring
            {
                HandleError(PrinterErrorManager.GetRandomError());
                Status = PrinterStatus.Error;
            }
            else
            {
                _logManager.LogJob(job, Name, processingTime);
                Status = PrinterStatus.Ready;
            }
        }

        public virtual void HandleError(PrinterError error)
        {
            Status = PrinterStatus.Error;
            _logManager.LogError(Name, error);
        }
    }


}
