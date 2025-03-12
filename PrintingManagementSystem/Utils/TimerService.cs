using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingManagementSystem.Utils
{
    public static class TimerService
    {
        public static TimeSpan MeasureExecutionTime(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        public static T MeasureExecutionTime<T>(Func<T> func, out TimeSpan executionTime)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            T result = func();
            stopwatch.Stop();
            executionTime = stopwatch.Elapsed;
            return result;
        }
    }
}
