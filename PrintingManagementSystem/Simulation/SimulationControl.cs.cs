using PrintingManagementSystem.Core;
using PrintingManagementSystem.Services;
using System;
using System.Threading;

namespace PrintingManagementSystem.Simulation
{
    public class SimulationControl
    {
        private readonly JobSimulator _jobSimulator;
        private readonly PrintManager _printManager;
        private readonly PrinterStatusService _statusService;
        private readonly ErrorRecoveryService _errorService;
        private bool _isRunning;
        private Thread _monitoringThread;

        public SimulationControl(JobSimulator jobSimulator, PrintManager printManager,
            PrinterStatusService statusService, ErrorRecoveryService errorService)
        {
            _jobSimulator = jobSimulator;
            _printManager = printManager;
            _statusService = statusService;
            _errorService = errorService;
        }

        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;

            _jobSimulator.StartSimulation();

            _monitoringThread = new Thread(() =>
            {
                while (_isRunning)
                {
                    _printManager.ProcessAllPrinters();
                    _errorService.RecoverPrinters();
                    _statusService.UpdatePrinterStatus();
                    Thread.Sleep(5000); // Update simulation every 5 seconds
                }
            });

            _monitoringThread.Start();
            Console.WriteLine("[SimulationControl] Simulation started.");
        }

        public void Stop()
        {
            _isRunning = false;
            _jobSimulator.StopSimulation();
            _monitoringThread?.Join();
            Console.WriteLine("[SimulationControl] Simulation stopped.");
        }
    }

}
