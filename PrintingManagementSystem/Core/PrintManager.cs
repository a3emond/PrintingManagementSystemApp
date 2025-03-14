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

        public void AddPrintJob(PrintJob job)
        {
            _logManager.LogJobAdded(job);
            _jobQueue.AddJob(job);
            DispatchJobs();
        }

        private void DispatchJobs()
        {
            while (!_jobQueue.IsEmpty)
            {
                var availablePrinter = _printers
                    .Where(p => p.Status == PrinterStatus.Ready)
                    .OrderBy(p => p.PrinterQueue.IsEmpty ? 0 : 1) // Prioritize idle printers
                    .FirstOrDefault();

                if (availablePrinter != null)
                {
                    PrintJob job = _jobQueue.GetNextJob();
                    availablePrinter.AssignJob(job);

                    _logManager.LogJobAssignment(availablePrinter.Name, job);

                    // Trigger JobAssigned event
                    JobAssigned?.Invoke(this, new PrintJobEventArgs(job, availablePrinter.Name));
                }
                else
                {
                    _logManager.LogNoAvailablePrinters();
                    break;
                }
            }
        }
        public void ProcessAllPrinters()
        {
            foreach (var printer in _printers)
            {
                if (printer.Status == PrinterStatus.Ready && !printer.PrinterQueue.IsEmpty)
                {
                    printer.ProcessJob();
                }
            }
            DispatchJobs(); // Attempt to assign more jobs if printers become available
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
