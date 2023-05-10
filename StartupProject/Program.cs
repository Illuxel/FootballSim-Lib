using DatabaseLayer;
using System;
using System.Drawing;
using BusinessLogicLayer;
using BusinessLogicLayer.Services;
using DatabaseLayer.Repositories;
using DatabaseLayer.Services;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using BusinessLogicLayer.Services.TransferMarketManager;

namespace StartupProject
{
    internal class Program
    {
       
        public static void Main(string[] args)
        {
            //TeamRatingWinCoeff repository testing
            
            /*var teamRWC = new TeamRatingWinCoeff();
            teamRWC.TeamId = "015834FD9556AAEC44DE54CDE350235B";
            teamRWC.Season = "november";
            var teamRep = new TeamRatingWinCoeffRepository();
            teamRep.Insert(teamRWC);
            var result = teamRep.RetrieveOne("015834FD9556AAEC44DE54CDE350235B","november");
            teamRWC.WinCoeff = 93;
            teamRep.Update(teamRWC);
            var result_2 = teamRep.Retrieve();
            result = teamRep.RetrieveOne("015834FD9556AAEC44DE54CDE350235B","november");*/

            
            //Team UpdateRating method testing
            
            /*var teamRep = new TeamRepository();
            var teamTEST = teamRep.Retrive("015834FD9556AAEC44DE54CDE350235B");
            teamRep.UpdateRating("015834FD9556AAEC44DE54CDE350235B",2);
            var teamTEST1 = teamRep.Retrive("015834FD9556AAEC44DE54CDE350235B");
            Console.WriteLine(teamTEST1.CurrentInterlRatingPosition);*/

        }
    }
}
