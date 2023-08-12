using BusinessLogicLayer.Services;
using DatabaseLayer.Repositories;
using System;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var players = new PlayerRepository().Retrieve("015834FD9556AAEC44DE54CDE350235B");
            var serv = new PlayerPriceCalculator();
            foreach (var player in players)
            {
                Console.WriteLine();
                Console.WriteLine($"{player.PersonID} {player.Rating} \n{serv.GetPlayerPrice(player)}\n{serv.GetPlayerSalary(player)}\n");
                Console.WriteLine();
            }
        }
    }
}
