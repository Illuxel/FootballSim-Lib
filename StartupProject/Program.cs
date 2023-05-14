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
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace StartupProject
{
    internal class Program
    {
       
        public static void Main(string[] args)
        {
            var dataDo = new SeasonValueCreator();
            var upd = new RatingActualizer();
            var teamRating = new TeamRatingWinCoeff();
            var teamRatingRep = new TeamRatingWinCoeffRepository();

            var list = new List<string>() { "015834FD9556AAEC44DE54CDE350235B", "B5551778D1672E4E544F32BFFAD52BA6", "678065FDDB06C590A0D0F9EDC2B5196F", "3809238CBB4AA8274B555A7B0750FCE5" };
            var listNums = new List<double>() { 4, 30, 27, 10};
            int index = 0;
            foreach (var item in list)
            {
                teamRating.TeamId = item;
                teamRating.Season = dataDo.GetSeason(2023);
                teamRating.WinCoeff = listNums[index];
                teamRatingRep.Insert(teamRating);

                teamRating.Season = dataDo.GetSeason(2022);
                teamRating.WinCoeff = listNums[index];
                teamRatingRep.Insert(teamRating);

                teamRating.Season = dataDo.GetSeason(2021);
                teamRating.WinCoeff = listNums[index];
                teamRatingRep.Insert(teamRating);

                teamRating.Season = dataDo.GetSeason(2020);
                teamRating.WinCoeff = listNums[index];
                teamRatingRep.Insert(teamRating);

                teamRating.Season = dataDo.GetSeason(2019);
                teamRating.WinCoeff = listNums[index];
                teamRatingRep.Insert(teamRating);

                teamRating.Season = dataDo.GetSeason(2018);
                teamRating.WinCoeff = 100;
                teamRatingRep.Insert(teamRating);
                index++;
            }
/*
            ///1 Chealsea
            teamRating.TeamId = "015834FD9556AAEC44DE54CDE350235B";

            teamRating.Season = dataDo.GetSeason(2023);
            teamRating.WinCoeff = 10;
            teamRatingRep.Insert(teamRating);

            teamRating.Season = dataDo.GetSeason(2022);
            teamRating.WinCoeff = 10;
            teamRatingRep.Insert(teamRating);

            teamRating.Season = dataDo.GetSeason(2021);

            teamRating.WinCoeff = 10;
            teamRatingRep.Insert(teamRating);

            teamRating.Season = dataDo.GetSeason(2020);
            teamRating.WinCoeff = 10;
            teamRatingRep.Insert(teamRating);

            teamRating.Season = dataDo.GetSeason(2019);
            teamRating.WinCoeff = 10;
            teamRatingRep.Insert(teamRating);

            teamRating.Season = dataDo.GetSeason(2018);
            teamRating.WinCoeff = 10;
            teamRatingRep.Insert(teamRating);

            ///2 Liverpool

            teamRating.TeamId = "B5551778D1672E4E544F32BFFAD52BA6";
            teamRating.Season = dataDo.GetSeason(2023);
            teamRating.WinCoeff = 9;
            teamRatingRep.Insert(teamRating);

            teamRating.Season = dataDo.GetSeason(2022);
            teamRating.WinCoeff = 9;
            teamRatingRep.Insert(teamRating);

            ///3 Man City

            teamRating.TeamId = "678065FDDB06C590A0D0F9EDC2B5196F";
            teamRating.Season = dataDo.GetSeason(2023);
            teamRating.WinCoeff = 100;
            teamRatingRep.Insert(teamRating);

            teamRating.Season = "2021/2022";
            teamRating.WinCoeff = 8;
            teamRatingRep.Insert(teamRating);

            //4 Arsenal
            teamRating.TeamId = "3809238CBB4AA8274B555A7B0750FCE5";
            teamRating.Season = dataDo.GetSeason(2023);
            teamRating.WinCoeff = 7;
            teamRatingRep.Insert(teamRating);
            teamRating.Season = dataDo.GetSeason(2022);
            teamRating.WinCoeff = 7;
            teamRatingRep.Insert(teamRating);
            teamRating.Season = dataDo.GetSeason(2021);
            teamRating.WinCoeff = 7;
            teamRatingRep.Insert(teamRating);*/

            DateTime date = DateTime.Now;

            upd.Actualize(date);
        }
    }
}
