using PrintingManagementSystem.Core;
using PrintingManagementSystem.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrintingManagementSystem.Simulation
{
    public class JobGenerationService
    {
        /*
         * The JobGenerationService will serve as an alternative to JobSimulator,
         * generating print jobs at configurable intervals for simulations.
         * It will be used for batch job generation, allowing manual control of job creation in testing.
         */
        private readonly PrintManager _printManager;
        private readonly Random _random;
        private bool _isRunning;
        private CancellationTokenSource _cancellationTokenSource;
        private int _batchSize;

        public JobGenerationService(PrintManager printManager, int batchSize = 5)
        {
            _printManager = printManager;
            _random = new Random();
            _batchSize = batchSize;
        }

        public void StartGenerating()
        {
            if (_isRunning) return;
            _isRunning = true;
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() => GenerateBatchJobsAsync(_cancellationTokenSource.Token));
            Console.WriteLine("[JobGenerationService] Batch job generation started.");
        }

        public void StopGenerating()
        {
            _isRunning = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            Console.WriteLine("[JobGenerationService] Batch job generation stopped.");
        }

        private async Task GenerateBatchJobsAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    for (int i = 0; i < _batchSize; i++)
                    {
                        var job = new PrintJob(
                            documentName: $"Batch_Doc_{_random.Next(1000)}",
                            pages: _random.Next(1, 20),
                            paperSize: _random.Next(2) == 0 ? "A4" : "Letter",
                            isColor: _random.Next(2) == 1,
                            priority: (JobPriority)_random.Next(1, 4) // Random priority
                        );

                        Console.WriteLine($"[JobGenerationService] Generated batch job: {job}");
                        await _printManager.AddPrintJobAsync(job);
                    }

                    await Task.Delay(_random.Next(10000, 20000), token); // Generate a batch every 10-20 seconds
                }
            }
            catch (TaskCanceledException) { /* Handle graceful cancellation */ }
        }
    }
}