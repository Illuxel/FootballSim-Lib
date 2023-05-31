using BusinessLogicLayer.Services;
using System;
using System.Diagnostics;

namespace StartupProject
{
    internal class Program
    {
       
        public static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            //TeamForMatchCreatorTest
            sw.Start();
            BudgetManager budgetManager = new BudgetManager();
            budgetManager.PaySalary();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds.ToString());
        }
    }
}
