using PrintingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PrintingManagementSystem.UI
{
    public class CanvasControl : Panel
    {
        private readonly List<IPrinter> _printers;
        private readonly List<MovingJob> _movingJobs;
        private readonly Timer _animationTimer;
        private readonly Random _random;

        public CanvasControl()
        {
            _printers = new List<IPrinter>();
            _movingJobs = new List<MovingJob>();
            _random = new Random();

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
                _movingJobs.Add(new MovingJob(job, new Point(20, 50), GetPrinterLocation(targetPrinter)));
            }
        }

        private void MoveJobs()
        {
            foreach (var job in _movingJobs)
            {
                job.Position = new Point(job.Position.X + 5, job.Position.Y);
                if (job.Position.X >= job.Target.X)
                {
                    _movingJobs.Remove(job);
                    break;
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

            g.FillRectangle(statusBrush, new Rectangle(location, new Size(100, 50)));
            g.DrawString(printer.Name, DefaultFont, Brushes.White, location);
        }

        private Point GetPrinterLocation(IPrinter printer)
        {
            int index = _printers.IndexOf(printer);
            return new Point(500, 50 + (index * 100));
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
