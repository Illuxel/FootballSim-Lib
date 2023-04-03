using DatabaseLayer;
using System;
using System.Drawing;
using BusinessLogicLayer;
using BusinessLogicLayer.Services;
using DatabaseLayer.Repositories;

namespace StartupProject
{
    internal class Program
    {
       
        public static void Main(string[] args)
        {
            var mge = new MatchGeneratingExampleUsing();
            mge.GenerateMatch();
        }
    }
}
