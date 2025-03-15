using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PrintingManagementSystem.Data;

namespace PrintingManagementSystem.UI
{
    public class ErrorLogPanel : Panel
    {
        private readonly ListBox _errorLogList;
        private readonly Timer _updateTimer;
        private readonly LogManager _logManager;

        public ErrorLogPanel(LogManager logManager)
        {
            _logManager = logManager;
            this.Width = 700;
            this.Height = 300;
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;

            _errorLogList = new ListBox { Left = 5, Top = 5, Width = 600, Height = 300 };
            this.Controls.Add(_errorLogList);

            _updateTimer = new Timer { Interval = 2000 };
            _updateTimer.Tick += (sender, args) => RefreshErrorLogs();
            _updateTimer.Start();
        }

        private void RefreshErrorLogs()
        {
            _errorLogList.Items.Clear();

            var errorLogs = _logManager.GetErrorLogs().ToList();
            foreach (var error in errorLogs)
            {
                _errorLogList.Items.Add($"[ERROR] {error}");
            }

            if (_errorLogList.Items.Count > 0)
            {
                _errorLogList.TopIndex = _errorLogList.Items.Count - 1; // Auto-scroll to the latest entry
            }
        }
    }
}