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
