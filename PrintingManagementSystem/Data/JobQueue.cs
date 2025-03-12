using PrintingManagementSystem.Models;
using System;

namespace PrintingManagementSystem.Data
{
    public class JobQueue : PriorityCircularQueue<PrintJob>
    {
        public JobQueue(int capacity) : base(capacity) { }

        public void AddJob(PrintJob job)
        {
            Console.WriteLine($"[JobQueue] New job added: {job.DocumentName}, Priority: {job.Priority}");
            Enqueue(job);
        }

        public PrintJob GetNextJob()
        {
            if (IsEmpty)
            {
                Console.WriteLine("[JobQueue] No jobs available.");
                return null;
            }
            return Dequeue();
        }
    }
}
