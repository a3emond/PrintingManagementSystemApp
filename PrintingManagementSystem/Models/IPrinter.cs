﻿using PrintingManagementSystem.Data;

namespace PrintingManagementSystem.Models
{
    public interface IPrinter
    {
        string Name { get; }
        PrinterStatus Status { get; set; }
        PrinterQueue PrinterQueue { get; }
        void AssignJob(PrintJob job);
        void ProcessJob();
        void HandleError(PrinterError error);
    }
}
