using PrintingManagementSystem.Data;
using PrintingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrintingManagementSystem.Services
{
    public class ErrorRecoveryService
    {
        private readonly List<IPrinter> _printers;
        private readonly LogManager _logManager;

        public ErrorRecoveryService(List<IPrinter> printers, LogManager logManager)
        {
            _printers = printers;
            _logManager = logManager;
        }

        public void RecoverPrinters()
        {
            foreach (var printer in _printers.Where(p => p.Status == PrinterStatus.Error))
            {
                _logManager.LogRecoveryAttempt(printer.Name);
                // Simulate user intervention
                // wait for 5 seconds 
                System.Threading.Thread.Sleep(5000);
                // Log recovery success
                _logManager.LogRecovery(printer.Name);
                printer.Status = PrinterStatus.Ready; // Set printer status to ready
            }
        }
    }
}
