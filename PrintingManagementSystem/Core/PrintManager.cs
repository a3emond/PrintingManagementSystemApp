using PrintingManagementSystem.Data;
using PrintingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrintingManagementSystem.Core
{
    public class PrintManager
    {
        private readonly List<IPrinter> _printers;
        private readonly JobQueue _jobQueue;
        private readonly LogManager _logManager;

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
            Console.WriteLine($"[PrintManager] Registered printer: {printer.Name}");
        }

        public void AddPrintJob(PrintJob job)
        {
            Console.WriteLine($"[PrintManager] Job added: {job.DocumentName} (Priority: {job.Priority})");
            _jobQueue.AddJob(job);
            DispatchJobs(); // Try assigning the job immediately
        }

        private void DispatchJobs()
        {
            while (!_jobQueue.IsEmpty)
            {
                var availablePrinter = _printers
                    .Where(p => p.Status == PrinterStatus.Ready)
                    .OrderBy(p => p.JobQueue.IsEmpty ? 0 : 1) // Prioritize idle printers
                    .FirstOrDefault();

                if (availablePrinter != null)
                {
                    PrintJob job = _jobQueue.GetNextJob();
                    availablePrinter.AssignJob(job);
                    Console.WriteLine($"[PrintManager] Assigned {job.DocumentName} to {availablePrinter.Name}");
                }
                else
                {
                    Console.WriteLine("[PrintManager] No available printers, job remains in queue.");
                    break;
                }
            }
        }

        public void ProcessAllPrinters()
        {
            foreach (var printer in _printers)
            {
                if (printer.Status == PrinterStatus.Ready && !printer.JobQueue.IsEmpty)
                {
                    printer.ProcessJob();
                }
            }
            DispatchJobs(); // Re-attempt dispatching in case printers become free
        }

        public void HandlePrinterErrors()
        {
            foreach (var printer in _printers.Where(p => p.Status == PrinterStatus.Error))
            {
                Console.WriteLine($"[PrintManager] Attempting to recover {printer.Name}");

                // Simulate user intervention or auto-recovery
                printer.Status = PrinterStatus.Ready;
                _logManager.LogError(printer.Name, PrinterError.None);

                Console.WriteLine($"[PrintManager] {printer.Name} is back online.");
            }
        }
    }
}
