﻿using PrintingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PrintingManagementSystem.Core;

namespace PrintingManagementSystem.UI
{
    public class CanvasControl : Panel
    {
        private readonly List<IPrinter> _printers;
        private readonly List<MovingJob> _movingJobs;
        private readonly Timer _animationTimer;
        private readonly Random _random;
        private readonly PrintManager _printManager;

        public CanvasControl(PrintManager printManager)
        {
            _printers = new List<IPrinter>();
            _movingJobs = new List<MovingJob>();
            _random = new Random();
            _printManager = printManager;

            this.DoubleBuffered = true;
            this.Paint += Canvas_Paint;

            _animationTimer = new Timer { Interval = 100 };
            _animationTimer.Tick += (sender, args) => MoveJobs();
            _animationTimer.Start();
        }

        public void RegisterPrinters(List<IPrinter> printers)
        {
            _printers.Clear();
            _printers.AddRange(printers);
            Invalidate();
        }

        public void AddJobAnimation(PrintJob job, string printerName)
        {
            var targetPrinter = _printers.Find(p => p.Name == printerName);
            if (targetPrinter != null)
            {
                int startY = _random.Next(50, this.Height - 50);
                _movingJobs.Add(new MovingJob(job, new Point(50, startY), GetPrinterLocation(targetPrinter)));
            }
        }

        private void MoveJobs()
        {
            for (int i = _movingJobs.Count - 1; i >= 0; i--)
            {
                var job = _movingJobs[i];

                // Move job towards target in small steps
                int step = 10;
                int dx = Math.Sign(job.Target.X - job.Position.X) * Math.Min(step, Math.Abs(job.Target.X - job.Position.X));
                int dy = Math.Sign(job.Target.Y - job.Position.Y) * Math.Min(step, Math.Abs(job.Target.Y - job.Position.Y));

                job.Position = new Point(job.Position.X + dx, job.Position.Y + dy);

                // If job reached the target, remove it
                if (job.Position == job.Target)
                {
                    _movingJobs.RemoveAt(i);
                }
            }
            Invalidate();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Draw printers as nodes
            int yOffset = 50;
            foreach (var printer in _printers)
            {
                DrawPrinter(g, printer, new Point(500, yOffset));
                yOffset += 100;
            }

            // Draw moving jobs
            foreach (var job in _movingJobs)
            {
                g.FillRectangle(Brushes.Blue, new Rectangle(job.Position, new Size(20, 20)));
                g.DrawString(job.Job.DocumentName, DefaultFont, Brushes.White, job.Position);
            }

            // Draw queue states
            DrawQueueStates(g);
        }

        private void DrawQueueStates(Graphics g)
        {
            int mainQueueXOffset = 50;
            int mainQueueYOffset = 50;

            // Draw main queue
            var mainQueue = GetMainQueue();
            g.DrawString("Main Queue:", DefaultFont, Brushes.Black, mainQueueXOffset, mainQueueYOffset);
            mainQueueYOffset += 20;
            if (mainQueue.Any())
            {
                foreach (var job in mainQueue)
                {
                    g.DrawString($"{job.DocumentName} (Priority: {job.Priority})", DefaultFont, Brushes.Black, mainQueueXOffset, mainQueueYOffset);
                    mainQueueYOffset += 20;
                }
            }

            // Draw printer queues
            int printerYOffset = 50;
            foreach (var printer in _printers)
            {
                int printerQueueXOffset = 700; 
                var queueState = printer.PrinterQueue.GetQueueState();
                g.DrawString($"Queue for {printer.Name}:", DefaultFont, Brushes.Black, printerQueueXOffset, printerYOffset);
                printerYOffset += 20;
                if (queueState.Any())
                {
                    foreach (var job in queueState)
                    {
                        g.DrawString($"{job.DocumentName} (Priority: {job.Priority})", DefaultFont, Brushes.Black, printerQueueXOffset, printerYOffset);
                        printerYOffset += 20;
                    }
                }
                printerYOffset += 20; // Add extra space between printers
            }
        }

        private void DrawPrinter(Graphics g, IPrinter printer, Point location)
        {
            Brush statusBrush;
            switch (printer.Status)
            {
                case PrinterStatus.Ready:
                    statusBrush = Brushes.Green;
                    break;
                case PrinterStatus.Busy:
                    statusBrush = Brushes.Yellow;
                    break;
                case PrinterStatus.Error:
                    statusBrush = Brushes.Red;
                    break;
                default:
                    statusBrush = Brushes.Gray;
                    break;
            }

            // Printer Rectangle
            Rectangle rect = new Rectangle(location, new Size(150, 70)); // Increased size

            // Fill rectangle
            g.FillRectangle(statusBrush, rect);

            // Draw a black border around the printer for visibility
            g.DrawRectangle(Pens.Black, rect);

            // Draw printer name
            using (Font font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold))
            {
                g.DrawString(printer.Name, font, Brushes.Black, location.X + 10, location.Y + 20);
            }
        }

        private Point GetPrinterLocation(IPrinter printer)
        {
            int index = _printers.IndexOf(printer);
            return new Point(500, 50 + (index * 100));
        }

        private List<PrintJob> GetMainQueue()
        {
            return _printManager.GetMainQueueState();
        }
    }

    public class MovingJob
    {
        public PrintJob Job { get; }
        public Point Position { get; set; }
        public Point Target { get; }

        public MovingJob(PrintJob job, Point start, Point target)
        {
            Job = job;
            Position = start;
            Target = target;
        }
    }
}
