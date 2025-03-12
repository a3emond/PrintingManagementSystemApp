using PrintingManagementSystem.Data;
using PrintingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrintingManagementSystem.Services
{
    public class PrintJobDispatcher
    {
        private readonly List<IPrinter> _printers;
        private readonly LogManager _logManager;

        public PrintJobDispatcher(List<IPrinter> printers, LogManager logManager)
        {
            _printers = printers;
            _logManager = logManager;
        }

        public void DispatchJob(PrintJob job)
        {
            var suitablePrinters = _printers
                .Where(p => p.Status == PrinterStatus.Ready)
                .OrderBy(p => p.JobQueue.IsEmpty ? 0 : 1) // Prioritize idle printers
                .ToList();

            if (suitablePrinters.Any())
            {
                var selectedPrinter = suitablePrinters.First();
                selectedPrinter.AssignJob(job);
                _logManager.LogJob(job, selectedPrinter.Name, TimeSpan.Zero);
                Console.WriteLine($"[PrintJobDispatcher] Assigned {job.DocumentName} to {selectedPrinter.Name}");
            }
            else
            {
                Console.WriteLine($"[PrintJobDispatcher] No available printers for {job.DocumentName}");
            }
        }
    }
}
