using PrintingManagementSystem.Data;
using System.Threading.Tasks;

namespace PrintingManagementSystem.Models
{
    public interface IPrinter
    {
        string Name { get; }
        PrinterStatus Status { get; set; }
        PrinterQueue PrinterQueue { get; }

        Task AssignJobAsync(PrintJob job);
        Task ProcessJobAsync();
        Task HandleErrorAsync(PrinterError error);
    }
}