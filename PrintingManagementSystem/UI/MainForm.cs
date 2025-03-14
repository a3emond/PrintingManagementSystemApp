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
        private readonly SimulationControl _simulationControl;
        private readonly LogManager _logManager;
        private CanvasControl _canvas;

        public MainForm()
        {
            InitializeComponent();

            _logManager = new LogManager();
            _printManager = new PrintManager(jobQueueCapacity: 10, _logManager);
            _simulationControl = new SimulationControl(_printManager,
                new PrinterStatusService(_printManager.GetPrinters()),
                new ErrorRecoveryService(_printManager.GetPrinters(), _logManager));


            SetupUI();
            RegisterPrinters();

            // Subscribe to job assignment event
            _printManager.JobAssigned += OnJobAssigned;
        }

        private void SetupUI()
        {
            this.Text = "Printing Management System";
            this.Width = 760;
            this.Height = 900;
            this.FormClosing += (sender, args) => _simulationControl.Stop();

            // Add Start/Stop Simulation Buttons
            var startButton = new Button { Text = "Start Simulation", Left = 20, Top = 20, Width = 120 };
            startButton.Click += (sender, args) => _simulationControl.Start();
            Controls.Add(startButton);

            var stopButton = new Button { Text = "Stop Simulation", Left = 160, Top = 20, Width = 120 };
            stopButton.Click += (sender, args) => _simulationControl.Stop();
            Controls.Add(stopButton);

            // Initialize and add Canvas
            _canvas = new CanvasControl { Left = 20, Top = 60, Width = 700, Height = 400 };
            _canvas.BackColor = System.Drawing.Color.CornflowerBlue;
            Controls.Add(_canvas);

            
            // Add Log Panel
            var logPanel = new LogPanel(_logManager) { Left = 20, Top = 470, Width = 700, Height = 300 };
            Controls.Add(logPanel);
        }

        private void RegisterPrinters()
        {
            var printers = new List<IPrinter>
            {
                new LaserPrinter("Laser Printer 1", _logManager),
                new InkjetPrinter("Inkjet Printer 1", _logManager)
            };

            foreach (var printer in printers)
            {
                _printManager.RegisterPrinter(printer);
            }

            // Pass printers to canvas for rendering
            _canvas.RegisterPrinters(_printManager.GetPrinters());
            _canvas.Invalidate();
        }

        private void OnJobAssigned(object sender, PrintJobEventArgs e)
        {
            _canvas.AddJobAnimation(e.Job, e.PrinterName);
        }
    }

}
