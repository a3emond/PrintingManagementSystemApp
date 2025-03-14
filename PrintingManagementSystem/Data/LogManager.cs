using PrintingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace PrintingManagementSystem.Data
{


    public class LogManager
    {
        private readonly List<string> _jobLogs;
        private readonly List<string> _errorLogs;
        private readonly string _logDirectory = "Logs";
        private readonly string _jobLogFile = "Logs/JobLog.txt";
        private readonly string _errorLogFile = "Logs/ErrorLog.txt";
        private readonly string _jobLogCsv = "Logs/JobLog.csv";
        private readonly string _errorLogCsv = "Logs/ErrorLog.csv";

        public LogManager()
        {
            _jobLogs = new List<string>();
            _errorLogs = new List<string>();

            // Ensure log directory and files exist
            Directory.CreateDirectory(_logDirectory);
            EnsureFileExists(_jobLogFile);
            EnsureFileExists(_errorLogFile);
        }

        private void EnsureFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { }
            }
        }

        // Log message
        public void LogMessage(string message)
        {
            string logEntry = $"[{DateTime.Now}] {message}";
            _jobLogs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        // Log registration of printer
        public void LogPrinterRegistration(string printerName)
        {
            string logEntry = $"[{DateTime.Now}] Printer Registered: {printerName}";
            _jobLogs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        // Log job added 
        public void LogJobAdded(PrintJob job)
        {
            string logEntry = $"[{DateTime.Now}] Job added: {job.DocumentName} (Priority: {job.Priority})";
            _jobLogs.Add(logEntry);
            Console.WriteLine(logEntry);
        }
        // Log Start Printing
        public void LogStartPrinting(string printerName)
        {
            string logEntry = $"[{DateTime.Now}] [Printer: {printerName}] Printing Started";
            _jobLogs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        public void LogJob(PrintJob job, string printerName, TimeSpan processingTime)
        {
            string logEntry = $"[{DateTime.Now}] [Printer: {printerName}] Completed Job: {job.DocumentName} | Pages: {job.Pages} | Processing Time: {processingTime.TotalSeconds} sec";
            _jobLogs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        public void LogError(string printerName, PrinterError errorType)
        {
            string logEntry = $"[{DateTime.Now}] [Printer: {printerName}] ERROR: {errorType}";
            _errorLogs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        // Log Recovery attempt
        public void LogRecoveryAttempt(string printerName)
        {
            string logEntry = $"[{DateTime.Now}] [Printer: {printerName}] Attempting Recovery...";
            _errorLogs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        // Log Recovered printer
        public void LogRecovery(string printerName)
        {
            string logEntry = $"[{DateTime.Now}] [Printer: {printerName}] Recovery Successful";
            _errorLogs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        // Log Job Assignment
        public void LogJobAssignment(string printerName, PrintJob job)
        {
            string logEntry = $"[{DateTime.Now}] [Printer: {printerName}] Assigned Job: {job.DocumentName} | Pages: {job.Pages}";
            _jobLogs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        // Log no available printers
        public void LogNoAvailablePrinters()
        {
            string logEntry = $"[{DateTime.Now}] [PrintManager] No available printers, job remains in queue.";
            _jobLogs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        // GetJobLogs and GetErrorLogs methods are used in the LogPanel class
        public List<string> GetJobLogs() => _jobLogs;
        public List<string> GetErrorLogs() => _errorLogs;

        public void SaveLogs()
        {
            File.AppendAllLines(_jobLogFile, _jobLogs);
            File.AppendAllLines(_errorLogFile, _errorLogs);
            Console.WriteLine("[LogManager] Logs saved successfully.");
        }

        public void ExportLogsToCsv()
        {
            File.AppendAllLines(_jobLogCsv, _jobLogs);
            File.AppendAllLines(_errorLogCsv, _errorLogs);
            Console.WriteLine("[LogManager] Logs exported to CSV.");
        }
    }

}
