using PrintingManagementSystem.Core;
using PrintingManagementSystem.Data;
using PrintingManagementSystem.Models;
using PrintingManagementSystem.Services;
using PrintingManagementSystem.Simulation;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PrintingManagementSystem.UI
{
    public partial class MainForm : Form
    {
        private readonly PrintManager _printManager;
        private readonly JobSimulator _jobSimulator;
        private readonly SimulationControl _simulationControl;
        private readonly LogManager _logManager;
        private readonly List<PrinterInfoPanel> _printerPanels;

        public MainForm()
        {
            InitializeComponent();

            _logManager = new LogManager();
            _printManager = new PrintManager(jobQueueCapacity: 10, _logManager);
            _jobSimulator = new JobSimulator(_printManager);
            _simulationControl = new SimulationControl(_jobSimulator, _printManager,
                new PrinterStatusService(_printManager.GetPrinters()),
                new ErrorRecoveryService(_printManager.GetPrinters(), _logManager));

            _printerPanels = new List<PrinterInfoPanel>();

            SetupUI();
            RegisterPrinters();
        }

        private void SetupUI()
        {
            this.Text = "Printing Management System";
            this.Width = 900;
            this.Height = 600;
            this.FormClosing += (sender, args) => _simulationControl.Stop();

            // Add Start/Stop Simulation Buttons
            var startButton = new Button { Text = "Start Simulation", Left = 20, Top = 20, Width = 120 };
            startButton.Click += (sender, args) => _simulationControl.Start();
            Controls.Add(startButton);

            var stopButton = new Button { Text = "Stop Simulation", Left = 160, Top = 20, Width = 120 };
            stopButton.Click += (sender, args) => _simulationControl.Stop();
            Controls.Add(stopButton);

            // Add Canvas for animations
            var canvas = new CanvasControl { Left = 300, Top = 20, Width = 500, Height = 400 };
            Controls.Add(canvas);

            // Add Printer Info Panels
            var printerPanelContainer = new FlowLayoutPanel
            {
                Left = 20,
                Top = 60,
                Width = 250,
                Height = 400,
                AutoScroll = true
            };
            Controls.Add(printerPanelContainer);

            foreach (var printer in _printManager.GetPrinters())
            {
                var panel = new PrinterInfoPanel(printer);
                _printerPanels.Add(panel);
                printerPanelContainer.Controls.Add(panel);
            }

            // Add Log Panel
            var logPanel = new LogPanel(_logManager) { Left = 20, Top = 470, Width = 780, Height = 100 };
            Controls.Add(logPanel);
        }

        private void RegisterPrinters()
        {
            _printManager.RegisterPrinter(new LaserPrinter("Laser Printer 1", _logManager));
            _printManager.RegisterPrinter(new InkjetPrinter("Inkjet Printer 1", _logManager));
        }
    }
}
