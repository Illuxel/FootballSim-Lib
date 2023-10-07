using DatabaseLayer.Services;
using System;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var safe = LoadGameManager.GetInstance().Load("test");
            var juniorFinder = new JuniorFinder();
            var bestJuniorPlayer = juniorFinder.BestJuniorPlayerByTeam("015834FD9556AAEC44DE54CDE350235B");
            Console.WriteLine($"Best junior player: {bestJuniorPlayer.Person.Name} {bestJuniorPlayer.Person.Surname} {bestJuniorPlayer.Rating}");
        }
    }
}