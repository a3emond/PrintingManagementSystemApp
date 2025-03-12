using PrintingManagementSystem.Models;
using System;

namespace PrintingManagementSystem.Data
{
    public class PrinterQueue : PriorityCircularQueue<PrintJob>
    {
        public PrinterQueue(int capacity) : base(capacity) { }

        public void AssignJob(PrintJob job)
        {
            Console.WriteLine($"[PrinterQueue] Job assigned to printer queue: {job.DocumentName}");
            Enqueue(job);
        }

        public PrintJob ProcessNextJob()
        {
            if (IsEmpty)
            {
                Console.WriteLine("[PrinterQueue] No jobs to process.");
                return null;
            }
            return Dequeue();
        }
    }
}
