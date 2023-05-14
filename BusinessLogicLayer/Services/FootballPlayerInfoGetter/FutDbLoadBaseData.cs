using System;
using System.Net.Http;
using System.Collections.Generic;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System.Linq;
using Newtonsoft.Json;

namespace BusinessLogicLayer.FetchPlayerData
{

    internal class FutDBProcessing
    {
        List<JsonTeam> teamListWhichAddToDataBase = new List<JsonTeam>();
        public class JsonCountry
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class JsonTeam
        {
            public int id { get; set; }
            public string name { get; set; }
            public int? league { get; set; }
        }


        public class RootTeam
        {
            public List<JsonTeam> items { get; set; }
        }

        public class RootCountry
        {
            public List<JsonCountry> items { get; set; }
        }

        public string SaveDataFromAPI(string url, int pageCount)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("X-AUTH-TOKEN", "4413e956-c92e-43a5-ac00-1fc3bbf76f15");
                var buffer = "[";
                for (var i = 1; i < pageCount; i++)
                {
                    var data = new Uri(url + i);
                    if (pageCount == i + 1)
                    {
                        buffer += client.GetAsync(data).Result.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        buffer += client.GetAsync(data).Result.Content.ReadAsStringAsync().Result + ",";
                    }
                }
                return buffer + "]";

            }
        }

        public void FillCountryIntoDataBaseFromFutdb()
        {
            string json = SaveDataFromAPI("https://futdb.app/api/nations?page=", 8);

            List<RootCountry> root = JsonConvert.DeserializeObject<List<RootCountry>>(json);


            CountryRepository countryRepository = new CountryRepository();
            List<Country> countries = countryRepository.Retrieve();


            foreach (RootCountry oneRoot in root)
            {
                foreach (var externalCountry in oneRoot.items)
                {
                    if(countries.Where(item => item.ExtId == externalCountry.id).Count() == 0)
                    {
                        countryRepository.Insert(new Country() { Name = externalCountry.name, ExtId = externalCountry.id });
                    }
                }
            }

        }

        public void FillTeamNameFromFutdb()
        {
            string json = SaveDataFromAPI("https://futdb.app/api/clubs?page=", 35);

            List<RootTeam> root = JsonConvert.DeserializeObject<List<RootTeam>>(json);


            TeamRepository teamRepository = new TeamRepository();
            List<Team> teams = teamRepository.Retrieve();

            foreach (RootTeam oneRoot in root)
            {
                foreach (var externalTeam in oneRoot.items)
                {
                    foreach (Team teamDb in teams)
                    {
                        if (externalTeam.id != 0 && teamDb.ExtId == externalTeam.id)
                        {
                            teamDb.ExtName = externalTeam.name;
                            teamRepository.Update(teamDb);
                        }
                    }
                }
            }
        }
    }


}