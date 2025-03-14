﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PrintingManagementSystem.Data;

namespace PrintingManagementSystem.UI
{
    public class LogPanel : Panel
    {
        private readonly ListBox _logList;
        private readonly Timer _updateTimer;
        private readonly LogManager _logManager;

        public LogPanel(LogManager logManager)
        {
            _logManager = logManager;
            this.Width = 700;
            this.Height = 300;
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;

            _logList = new ListBox { Left = 5, Top = 5, Width = 690, Height = 300 };
            this.Controls.Add(_logList);

            _updateTimer = new Timer { Interval = 2000 };
            _updateTimer.Tick += (sender, args) => RefreshLogs();
            _updateTimer.Start();
        }



        private void RefreshLogs()
        {
            _logList.Items.Clear();

            var jobLogs = _logManager.GetJobLogs().ToList();
            foreach (var log in jobLogs)
            {
                _logList.Items.Add(log);
            }

            if (_logList.Items.Count > 0)
            {
                _logList.TopIndex = _logList.Items.Count - 1; // Auto-scroll to the latest entry
            }
        }
    }
}