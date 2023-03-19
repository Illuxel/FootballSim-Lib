using FootBalLife.Database;
using FootBalLife.Database.Repositories;
using System.Collections.Generic;
using System.Text.Json;

namespace Services.Services
{
    public class FootballPlayerInfoGetter
    {
        private string _baseUrlFifaApi = "https://futdb.app/api";
        private string _authToken
        {
            get { return "4413e956-c92e-43a5-ac00-1fc3bbf76f15"; }
        }

        private int _playerRoleId = 3;

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

        private int _contractPriceKoef = 100000;


        private TeamRepository _teamRepos;
        private PersonRepository _personRepos;
        private PlayerRepository _playerRepos;
        private ContractRepository _contractRepos;
        private SeasonValueCreator _seasonValueCreator;
        public FootballPlayerInfoGetter()
        {
            _teamRepos = new TeamRepository();
            _personRepos = new PersonRepository();
            _playerRepos = new PlayerRepository();
            _contractRepos = new ContractRepository();
            _seasonValueCreator = new SeasonValueCreator();
        }

        public void FillPlayerInfo(string currentSeason)
        {
            var leagueRepos = new LeagueRepository();
            var leagues = leagueRepos.Retrive();

           
            foreach(var league in leagues)
            {
                var teams = _teamRepos.Retrive(league.Id);
                foreach(var team in teams)
                {
                    //var players = getPlayers(team.ExtId); //
                    var players = getPlayers(0);
                    foreach(var extPlayer in players)
                    {
                        var person = new Person();
                        person.CurrentRoleID = _playerRoleId;
                        person.Name = extPlayer.GetFirstName();
                        person.Surname = extPlayer.LastName;
                        person.Birthday = extPlayer.GetBirthDate();

                        _personRepos.Insert(person);


                        var player = new Player();
                        player.PersonID = person.Id;
                        player.Passing = extPlayer.Passing;
                        player.Strike = extPlayer.Shooting;
                        player.PositionCode = extPlayer.Position;
                        player.Physics = extPlayer.Physicality != 0 ? extPlayer.Physicality  : extPlayer.Positioning;
                        player.Speed = extPlayer.Pace;
                        player.Dribbling = extPlayer.Dribbling != 0? extPlayer.Dribbling : extPlayer.Reflexes;

                        _playerRepos.Insert(player);


                        var contract = new Contract();
                        contract.SeasonFrom = currentSeason;
                        contract.SeasonTo = _seasonValueCreator.GetFutureSeason(currentSeason, 3);
                        contract.PersonId = person.Id;
                        contract.TeamId = team.Id;
                        contract.Price = extPlayer.Rating * _contractPriceKoef;

                        _contractRepos.Insert(contract);

                    }
                }
            }

           
        }

        //викликати endpoint https://futdb.app/api/players/search
        /* body:
        {
		  "name": "string",
		  "ratingMin": 0,
		  "ratingMax": 0,
		  "rating": 0,
		  "rarity": 0,
		  "position": "string",
		  "club": 0,
		  "league": 0,
		  "nation": 0,
		  "weakFoot": 0,
		  "skillMoves": 0
		}
         */
        private List<ExternalPlayer> getPlayers(int fifaTeamId)
        {
            var players = new List<ExternalPlayer>();

            var player = getExternalPlayer(_playerJsonExample);
            players.Add(player);
            return players;
        }

        private int getFifaLeagueId(int leagueId)
        {
            switch(leagueId)
            {
                case 1: return 13;
                case 2: return 53;
                case 3: return 16;
                case 4: return 31;
                case 5: return 19;
                case 6: return 308;
                case 7: return 10;
                case 8: return 4;
                case 9: return 332;
                case 10: return 66;
                default: return 0;
            }
        }

        private int getLeagueId(int fifaLeagueId)
        {
            switch (fifaLeagueId)
            {
                case 13: return 1;
                case 53: return 2;
                case 16: return 3;
                case 31: return 4;
                case 19: return 5;
                case 308: return 6;
                case 10: return 7;
                case 4: return 8;
                case 332: return 9;
                case 66: return 10;
                default: return 0;
            }
        }

        private ExternalPlayer getExternalPlayer(string json)
        {
            var player = JsonSerializer.Deserialize<ExternalPlayer>(json);
            return player;
        }

    }
}
