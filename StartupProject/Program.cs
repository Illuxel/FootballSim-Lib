using FootBalLife.Database.Repositories;
using System;

namespace StartupProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TeamRepository teamRepository = new TeamRepository();
            var teams = teamRepository.Retrive();
            foreach(var item in teams)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
