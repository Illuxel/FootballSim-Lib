using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
           var serv = new JuniorPersonGeneration();
            serv.GenerateNewJuniorPerson("1", 2020, new DateTime(2000, 1, 1), 1, "GG");
        }
    }
}
