using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StartupProject
{
    internal class Program
    {
       
        public static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            /*Contract c = new Contract();
            ContractRepository cr = new ContractRepository();
            for(int i = 0;i<10000;i++)
            {
                cr.Insert(c.GetNewContract());
            }
            sw.Start();*/
            BudgetManager budgetManager = new BudgetManager();
            budgetManager.PaySalary(new DateTime(2023,12,04));
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds.ToString());
        }
    }
}
