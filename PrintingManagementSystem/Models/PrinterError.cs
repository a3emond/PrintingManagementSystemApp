namespace PrintingManagementSystem.Models
{
    public enum PrinterError
    {
        None,             // No error
        OutOfPaper,       // Needs more paper
        PaperStuck,       // Paper jam
        NetworkError,     // Lost connection
        CorruptedFile,    // Cannot read job file
        OutOfInk          // Ink or toner depleted
    }

}
