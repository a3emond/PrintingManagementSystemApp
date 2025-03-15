using PrintingManagementSystem.Data;
using PrintingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrintingManagementSystem.Core
{
    public class PrintManager
    {
        private readonly List<IPrinter> _printers;
        private readonly JobQueue _jobQueue;
        private readonly LogManager _logManager;

        // Event for job assignment
        public event EventHandler<PrintJobEventArgs> JobAssigned;

        public PrintManager(int jobQueueCapacity, LogManager logManager)
        {
            _printers = new List<IPrinter>();
            _jobQueue = new JobQueue(jobQueueCapacity);
            _logManager = logManager;
        }

        public List<IPrinter> GetPrinters() => _printers;

        public void RegisterPrinter(IPrinter printer)
        {
            _printers.Add(printer);
            _logManager.LogPrinterRegistration(printer.Name);
        }

        public async Task AddPrintJobAsync(PrintJob job)
        {
            _logManager.LogJobAdded(job);
            _jobQueue.AddJob(job);
            // wait until 5 jobs are in the queue
            if (_jobQueue.Count >= 5)
                await DispatchJobsAsync();
        }

        private async Task DispatchJobsAsync()
        {
            while (!_jobQueue.IsEmpty)
            {
                var availablePrinters = _printers
                    .Where(p => p.Status != PrinterStatus.Error) // Exclude printers with errors
                    .OrderBy(p => p.PrinterQueue.IsEmpty ? 0 : 1) // Prioritize idle printers
                    .ToList();

                if (availablePrinters.Any())
                {
                    foreach (var printer in availablePrinters)
                    {
                        if (_jobQueue.IsEmpty)
                            break;

                        try
                        {
                            PrintJob job = _jobQueue.GetNextJob();
                            await printer.AssignJobAsync(job);

                            _logManager.LogJobAssignment(printer.Name, job);

                            // Trigger JobAssigned event
                            JobAssigned?.Invoke(this, new PrintJobEventArgs(job, printer.Name));

                            // Simulate slight delay before assigning the next job
                            await Task.Delay(100, CancellationToken.None);
                        }
                        catch (Exception ex)
                        {
                            _logManager.LogError(printer.Name, PrinterError.CorruptedFile);
                        }
                    }
                }
                else
                {
                    _logManager.LogNoAvailablePrinters();
                    break;
                }
            }
        }

        public async Task ProcessAllPrintersAsync()
        {
            List<Task> processingTasks = new List<Task>();

            foreach (var printer in _printers)
            {
                if (!printer.PrinterQueue.IsEmpty)
                {
                    processingTasks.Add(Task.Run(() => printer.ProcessJobAsync()));
                }
            }

            await Task.WhenAll(processingTasks);

            // Attempt to assign more jobs if printers become available
            await DispatchJobsAsync();
            
        }
        public List<PrintJob> GetMainQueueState()
        {
            return _jobQueue.GetQueueState();
        }
    }

    // Define the event args class
    public class PrintJobEventArgs : EventArgs
    {
        public PrintJob Job { get; }
        public string PrinterName { get; }

        public PrintJobEventArgs(PrintJob job, string printerName)
        {
            Job = job;
            PrinterName = printerName;
        }
    }
}
