using PrintingManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PrintingManagementSystem.UI
{
    public class PrinterInfoPanel : Panel
    {
        private readonly IPrinter _printer;
        private readonly Label _printerNameLabel;
        private readonly Label _statusLabel;
        private readonly Label _queueSizeLabel;
        private readonly Label _errorLabel;
        private readonly Timer _updateTimer;

        public PrinterInfoPanel(IPrinter printer)
        {
            _printer = printer;
            this.Width = 250;
            this.Height = 80;
            this.BackColor = Color.LightGray;
            this.BorderStyle = BorderStyle.FixedSingle;

            _printerNameLabel = new Label { Text = _printer.Name, Left = 10, Top = 5, Width = 200, Font = new Font("Arial", 10, FontStyle.Bold) };
            _statusLabel = new Label { Left = 10, Top = 25, Width = 200 };
            _queueSizeLabel = new Label { Left = 10, Top = 45, Width = 200 };
            _errorLabel = new Label { Left = 10, Top = 65, Width = 200, ForeColor = Color.Red };

            this.Controls.Add(_printerNameLabel);
            this.Controls.Add(_statusLabel);
            this.Controls.Add(_queueSizeLabel);
            this.Controls.Add(_errorLabel);

            _updateTimer = new Timer { Interval = 1000 };
            _updateTimer.Tick += (sender, args) => UpdatePanel();
            _updateTimer.Start();

            UpdatePanel();
        }

        private void UpdatePanel()
        {
            _statusLabel.Text = $"Status: {_printer.Status}";
            _queueSizeLabel.Text = $"Queue: {_printer.JobQueue.QueueSize} jobs";

            switch (_printer.Status)
            {
                case PrinterStatus.Ready:
                    this.BackColor = Color.LightGreen;
                    _errorLabel.Text = "";
                    break;
                case PrinterStatus.Busy:
                    this.BackColor = Color.Yellow;
                    _errorLabel.Text = "";
                    break;
                case PrinterStatus.Error:
                    this.BackColor = Color.LightCoral;
                    _errorLabel.Text = $"Error: {_printer.Status}";
                    break;
            }
        }
    }
}
