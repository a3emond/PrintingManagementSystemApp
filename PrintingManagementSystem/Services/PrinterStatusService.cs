using PrintingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrintingManagementSystem.Services
{
    public class PrinterStatusService
    {
        private readonly List<IPrinter> _printers;

        public PrinterStatusService(List<IPrinter> printers)
        {
            _printers = printers;
        }

        public void UpdatePrinterStatus()
        {
            foreach (var printer in _printers)
            {
                Console.WriteLine($"[PrinterStatusService] {printer.Name} - Status: {printer.Status}");
            }
        }

        public List<IPrinter> GetPrintersByStatus(PrinterStatus status)
        {
            return _printers.Where(p => p.Status == status).ToList();
        }
    }
}
