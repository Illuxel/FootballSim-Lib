using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Linq;

namespace StartupProject
{
    internal class Program
    {
       
        public static void Main(string[] args)
        {
            var calc = new PlayerPriceCalculator();
            var p = new PlayerRepository();
            var pl = p.Retrieve("3809238CBB4AA8274B555A7B0750FCE5");
            System.Console.WriteLine("Price = "+calc.GetPlayerPrice(pl.FirstOrDefault()));
            System.Console.WriteLine("Salary = " + calc.GetPlayerSalary(pl.FirstOrDefault()));
            Console.ReadKey();
        }
    }
}
