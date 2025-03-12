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

            if (chance < 15) return PrinterError.OutOfPaper;        // 15% chance
            if (chance < 30) return PrinterError.PaperStuck;        // 15% chance
            if (chance < 45) return PrinterError.NetworkError;      // 15% chance
            if (chance < 60) return PrinterError.CorruptedFile;     // 15% chance
            if (chance < 75) return PrinterError.OutOfInk;          // 15% chance

            return PrinterError.None; // 25% chance of no error
        }
    }

}
