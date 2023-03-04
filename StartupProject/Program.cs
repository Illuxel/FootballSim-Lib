using Services.Services;
using System;
using System.Diagnostics;

namespace StartupProject
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();





            var cmc = new ScheduleMatchGenerator();
            cmc.Generate(DateTime.Now.Year);






            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine($"Час виконання коду: {ts}"); 

        }
    }
}
