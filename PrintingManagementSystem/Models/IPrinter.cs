using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingManagementSystem.Models
{
    public interface IPrinter
    {
        string Name { get; }
        bool IsAvailable { get; }
        PrinterStatus Status { get; set; }
        Queue<PrintJob> JobQueue { get; }
        void AssignJob(PrintJob job);
        void ProcessJob();
    }
}
