using PrintingManagementSystem.Data;
using PrintingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task RecoverPrintersAsync(CancellationToken token)
        {
            foreach (var printer in _printers.Where(p => p.Status == PrinterStatus.Error))
            {
                _logManager.LogRecoveryAttempt(printer.Name);

                try
                {
                    await Task.Delay(5000, token); // Simulate user intervention

                    _logManager.LogRecovery(printer.Name);
                    printer.Status = PrinterStatus.Ready; // Set printer status to ready
                }
                catch (TaskCanceledException)
                {
                    _logManager.LogMessage($"[ErrorRecoveryService] Recovery process cancelled for {printer.Name}");
                }
            }
        }
    }
}