using PrintingManagementSystem.Models;
using System;


namespace PrintingManagementSystem.Core
{
    public static class PrinterErrorManager
    {
        private static readonly Random _random = new Random();

        public static PrinterError GetRandomError()
        {
            int chance = _random.Next(100); // Generate a number between 0-99

            if (chance < 20) return PrinterError.OutOfPaper;        // 20% chance
            if (chance < 40) return PrinterError.PaperStuck;        // 20% chance
            if (chance < 60) return PrinterError.NetworkError;      // 20% chance
            if (chance < 80) return PrinterError.CorruptedFile;     // 20% chance
            return PrinterError.OutOfInk;                           // 20% chance
        }
    }

}
