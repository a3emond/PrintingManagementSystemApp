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
                Console.WriteLine($"[ErrorRecoveryService] Recovering {printer.Name}");

                // Simulate user intervention
                printer.Status = PrinterStatus.Ready;
                _logManager.LogError(printer.Name, PrinterError.None);

                Console.WriteLine($"[ErrorRecoveryService] {printer.Name} is back online.");
            }
        }
    }
}
