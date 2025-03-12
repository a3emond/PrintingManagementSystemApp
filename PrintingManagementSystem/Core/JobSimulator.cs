using PrintingManagementSystem.Models;
using System;
using System.Threading;

namespace PrintingManagementSystem.Core
{

    public class JobSimulator
    {
        private readonly PrintManager _printManager;
        private readonly Random _random;
        private bool _isRunning;
        private Thread _simulationThread;

        public JobSimulator(PrintManager printManager)
        {
            _printManager = printManager;
            _random = new Random();
        }

        public void StartSimulation()
        {
            if (_isRunning) return;
            _isRunning = true;

            _simulationThread = new Thread(() =>
            {
                while (_isRunning)
                {
                    GenerateRandomJob();
                    Thread.Sleep(_random.Next(3000, 7000)); // Random job every 3-7 seconds
                }
            });

            _simulationThread.Start();
            Console.WriteLine("[JobSimulator] Simulation started.");
        }

        public void StopSimulation()
        {
            _isRunning = false;
            _simulationThread?.Join();
            Console.WriteLine("[JobSimulator] Simulation stopped.");
        }

        private void GenerateRandomJob()
        {
            var job = new PrintJob(
                documentName: $"Doc_{_random.Next(1000)}",
                pages: _random.Next(1, 20),
                paperSize: _random.Next(2) == 0 ? "A4" : "Letter",
                isColor: _random.Next(2) == 1,
                priority: (JobPriority)_random.Next(1, 4) // Random priority
            );

            Console.WriteLine($"[JobSimulator] Generated job: {job}");
            _printManager.AddPrintJob(job);
        }
    }

}
