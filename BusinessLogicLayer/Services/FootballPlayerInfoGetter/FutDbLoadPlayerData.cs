using DatabaseLayer;
using DatabaseLayer.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace BusinessLogicLayer.Services
{
    public class FutDbLoadPlayerData
    {
        private string _baseUrlFifaApi = "https://futdb.app/api";
        private string _authToken
        {
            get { return "4413e956-c92e-43a5-ac00-1fc3bbf76f15"; }
        }

        private static int _playerRoleId = 3;

        private string _playerJsonExample = @"{
              ""id"": 14112,
              ""resourceId"": 237067,
              ""resourceBaseId"": 237067,
              ""futBinId"": 62,
              ""futWizId"": null,
              ""firstName"": null,
              ""lastName"": null,
              ""name"": ""Pelé"",
              ""commonName"": ""Pelé"",
              ""height"": 173,
              ""weight"": 70,
              ""birthDate"": ""1940-10-23"",
              ""age"": 82,
              ""league"": 2118,
              ""nation"": 54,
              ""club"": 112658,
              ""rarity"": 12,
              ""traits"": [
                {
                  ""id"": 1,
                  ""name"": ""Technical Dribbler (CPU AI)""
                },
                {
                  ""id"": 2,
                  ""name"": ""Playmaker (CPU AI)""
                },
                {
                  ""id"": 3,
                  ""name"": ""Finesse Shot""
                },
                {
                  ""id"": 4,
                  ""name"": ""Solid Player""
                }
              ],
              ""specialities"": [],
              ""position"": ""CAM"",
              ""skillMoves"": 5,
              ""weakFoot"": 4,
              ""foot"": ""Right"",
              ""attackWorkRate"": ""High"",
              ""defenseWorkRate"": ""Med"",
              ""totalStats"": 516,
              ""totalStatsInGame"": 2513,
              ""color"": ""gold"",
              ""rating"": 98,
              ""ratingAverage"": 86,
              ""pace"": 95,
              ""shooting"": 96,
              ""passing"": 93,
              ""dribbling"": 96,
              ""defending"": 60,
              ""physicality"": 76,
              ""paceAttributes"": {
                ""acceleration"": 95,
                ""sprintSpeed"": 95
              },
              ""shootingAttributes"": {
                ""positioning"": 97,
                ""finishing"": 98,
                ""shotPower"": 94,
                ""longShots"": 94,
                ""volleys"": 95,
                ""penalties"": 93
              },
              ""passingAttributes"": {
                ""vision"": 97,
                ""crossing"": 90,
                ""freeKickAccuracy"": 89,
                ""shortPassing"": 96,
                ""longPassing"": 88,
                ""curve"": 89
              },
              ""dribblingAttributes"": {
                ""agility"": 94,
                ""balance"": 93,
                ""reactions"": 98,
                ""ballControl"": 97,
                ""dribbling"": 96,
                ""composure"": 98
              },
              ""defendingAttributes"": {
                ""interceptions"": 67,
                ""headingAccuracy"": 94,
                ""standingTackle"": 53,
                ""slidingTackle"": 49,
                ""defenseAwareness"": 55
              },
              ""physicalityAttributes"": {
                ""jumping"": 88,
                ""stamina"": 86,
                ""strength"": 76,
                ""aggression"": 59
              },
              ""goalkeeperAttributes"": {
                ""diving"": null,
                ""handling"": null,
                ""kicking"": null,
                ""positioning"": null,
                ""reflexes"": null,
                ""speed"": 95
              }
            }";


        private TeamRepository _teamRepos;
        private PersonRepository _personRepos;
        private PlayerRepository _playerRepos;
        private ContractRepository _contractRepos;
        private SeasonValueCreator _seasonValueCreator;
        private LeagueRepository _leagueRepos;
        private CountryRepository _countryRepository;
        public FutDbLoadPlayerData()
        {
            _teamRepos = new TeamRepository();
            _personRepos = new PersonRepository();
            _playerRepos = new PlayerRepository();
            _contractRepos = new ContractRepository();
            _seasonValueCreator = new SeasonValueCreator();
            _leagueRepos = new LeagueRepository();
            _countryRepository = new CountryRepository();
        }

        public void FillPlayerInfo(string currentSeason)
        {
            var leagues = _leagueRepos.Retrieve();
            var countries = _countryRepository.Retrieve();
            foreach (var league in leagues)
            {
                var teams = _teamRepos.Retrieve(league.Id);
                foreach(var team in teams)
                {
                    var players = getPlayers(team.ExtId);
                    foreach(var extPlayer in players)
                    {
                        var person = new Person();
                        person.CurrentRoleID = _playerRoleId;
                        person.Name = extPlayer.GetFirstName();
                        person.Surname = extPlayer.LastName;
                        person.Birthday = extPlayer.GetBirthDate();
                        person.CountryID = countries.Where(item => item.ExtId == extPlayer.CountryId).Select(value => value.Id).FirstOrDefault();

                        _personRepos.Insert(person);

                        var contract = new Contract();

                        contract.DateFrom = new DateTime(_seasonValueCreator.GetStartYear(currentSeason), 06, 1);
                        contract.DateTo = new DateTime(contract.DateFrom.Year + 3, 05, 31);
                        contract.PersonId = person.Id;
                        contract.TeamId = team.Id;
                        contract.Salary = getContractPrice(extPlayer.Rating);

                        _contractRepos.Insert(contract);
                         
                        var player = new Player();
                        player.PersonID = person.Id;
                        player.Passing = extPlayer.Passing;
                        player.Strike = extPlayer.Shooting;
                        player.PositionCode = extPlayer.Position;
                        player.Physics = extPlayer.Positioning == 0 ? extPlayer.Physicality  : extPlayer.Positioning;
                        player.Speed = extPlayer.Pace;
                        player.Dribbling = extPlayer.Reflexes == 0? extPlayer.Dribbling : extPlayer.Reflexes;
                        player.Defending = extPlayer.Defending;
                        player.Rating = extPlayer.Rating;
                        player.ContractID = contract.Id;
                        _playerRepos.Insert(player);
                    }
                }
            }
        }


        private int getContractPrice(int avaragePlayerRating)
        {
            if(avaragePlayerRating >= 90)
            {
                return avaragePlayerRating * 100000;
            }
            else if(avaragePlayerRating >= 85)
            {
                return avaragePlayerRating * 65000;
            }
            else if (avaragePlayerRating >= 80)
            {
                return avaragePlayerRating * 45000;
            }
            else if (avaragePlayerRating >= 75)
            {
                return avaragePlayerRating * 30000;
            }
            else if (avaragePlayerRating >= 70)
            {
                return avaragePlayerRating * 20000;
            }
            else if (avaragePlayerRating >= 60)
            {
                return avaragePlayerRating * 7000;
            }
            return avaragePlayerRating * 4500;
        }

        private List<ExternalPlayer> getPlayers(int fifaTeamId)
        {
            var players = new List<ExternalPlayer>();


            var url = string.Format("{0}/{1}", _baseUrlFifaApi, "players/search");
            var headers = new WebHeaderCollection();
            headers.Add("Accept", "application/json");
            headers.Add("Content-Type", "application/json");
            headers.Add("X-AUTH-TOKEN", _authToken);

            var request = (HttpWebRequest)WebRequest.Create(url);
            //request.Method = "GET";
            request.Method = "POST";
            request.Headers = headers;

            var body = new Dictionary<string, object>();
            body.Add("club", fifaTeamId);
            var jsonBody = JsonConvert.SerializeObject(body);
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonBody);
            }
             
            using var webResponse = request.GetResponse();
            using var webStream = webResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            if(data != null)
            {

                var responseResult = JsonConvert.DeserializeObject<FutDbResponseResult>(data);

                foreach (var item in responseResult.Items)
                {
                    var jsonData = JsonConvert.SerializeObject(item);
                    players.Add(getExternalPlayer(jsonData));
                }
            }

            var player = getExternalPlayer(_playerJsonExample);
            players.Add(player);
            return players;
        }

        private ExternalPlayer getExternalPlayer(string json)
        {
            var player = JsonConvert.DeserializeObject<ExternalPlayer>(json);
            return player;
        }

    }
}
