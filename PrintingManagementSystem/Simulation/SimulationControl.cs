using PrintingManagementSystem.Core;
using PrintingManagementSystem.Services;
using PrintingManagementSystem.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrintingManagementSystem.Simulation
{
    public class SimulationControl
    {
        private readonly PrintManager _printManager;
        private readonly PrinterStatusService _statusService;
        private readonly ErrorRecoveryService _errorService;
        private readonly Random _random;
        private CancellationTokenSource _cancellationTokenSource;

        public SimulationControl(PrintManager printManager, PrinterStatusService statusService, ErrorRecoveryService errorService)
        {
            _printManager = printManager;
            _statusService = statusService;
            _errorService = errorService;
            _random = new Random();
        }

        public void Start()
        {
            if (_cancellationTokenSource != null) return; // Already running
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() => GenerateRandomJobs(_cancellationTokenSource.Token));
            Task.Run(() => MonitorPrinters(_cancellationTokenSource.Token));

            Console.WriteLine("[SimulationControl] Simulation started.");
        }

        public void Stop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Token.WaitHandle.WaitOne(); // Wait for tasks to complete
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
                Console.WriteLine("[SimulationControl] Simulation stopped.");
            }
        }


        private async Task GenerateRandomJobs(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var job = new PrintJob(
                        documentName: $"Doc_{_random.Next(1000)}",
                        pages: _random.Next(1, 20),
                        paperSize: _random.Next(2) == 0 ? "A4" : "Letter",
                        isColor: _random.Next(2) == 1,
                        priority: (JobPriority)_random.Next(1, 4)
                    );

                    Console.WriteLine($"[SimulationControl] Generated job: {job}");
                    await _printManager.AddPrintJobAsync(job);

                    await Task.Delay(_random.Next(1000, 4000), token); // Wait between job generation
                }
            }
            catch (TaskCanceledException)
            {
                // Handle graceful stop
                _cancellationTokenSource?.Cancel(); 
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                Console.WriteLine(
                    "[SimulationControl] Job generation stopped. " +
                    "Printer status and error recovery services will continue to run."
                );
            }
        }

        private async Task MonitorPrinters(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await _printManager.ProcessAllPrintersAsync(); // should it be called there?
                    await _errorService.RecoverPrintersAsync(token);
                    _statusService.UpdatePrinterStatus();

                    await Task.Delay(5000, token); // Wait 5 seconds between status updates
                }
            }
            catch (TaskCanceledException)
            {
                // Handle graceful stop
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                Console.WriteLine(
                    "[SimulationControl] Printer monitoring stopped. " +
                    "Job generation will continue to run."
                );

            }
        }
    }
}
