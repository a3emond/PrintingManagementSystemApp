using PrintingManagementSystem.Data;
using PrintingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task DispatchJobAsync(PrintJob job)
        {
            var suitablePrinters = _printers
                .Where(p => p.Status == PrinterStatus.Ready)
                .OrderBy(p => p.PrinterQueue.IsEmpty ? 0 : 1) // Prioritize idle printers
                .ToList();

            if (suitablePrinters.Any())
            {
                var selectedPrinter = suitablePrinters.First();
                await selectedPrinter.AssignJobAsync(job);
                _logManager.LogJobAssignment(selectedPrinter.Name, job);
            }
            else
            {
                _logManager.LogMessage($"[PrintJobDispatcher] No available printers for {job.DocumentName}");
            }
        }
    }
}