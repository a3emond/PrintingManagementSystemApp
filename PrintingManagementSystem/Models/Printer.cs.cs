using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingManagementSystem.Core
{
    public abstract class Printer : IPrinter
    {
        public string Name { get; protected set; }
        public bool IsAvailable => Status == PrinterStatus.Ready && JobQueue.Count < Capacity;
        public PrinterStatus Status { get; set; }
        public Queue<PrintJob> JobQueue { get; private set; }
        public int Capacity { get; protected set; } // Max jobs in queue

        public Printer(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
            Status = PrinterStatus.Ready;
            JobQueue = new Queue<PrintJob>();
        }

        public void AssignJob(PrintJob job) => JobQueue.Enqueue(job);

        public abstract void ProcessJob();
    }

}
