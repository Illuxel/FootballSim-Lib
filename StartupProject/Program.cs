using BusinessLogicLayer.Scenario;
using System;


namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var gen = new JuniorGeneration();
            var gameDate = new DateTime(2021, 2, 8);
            gen.GenerateNewJuniorPerson("1", gameDate, 1, "TestName", "TestSurname");
        }
    }
}
