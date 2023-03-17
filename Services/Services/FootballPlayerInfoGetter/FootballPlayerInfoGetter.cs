using FootBalLife.Database;
using FootBalLife.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public class FootballPlayerInfoGetter
    {
        private string _baseUrlFifaApi = "https://futdb.app/api";
        private string _authToken
        {
            get { return "4413e956-c92e-43a5-ac00-1fc3bbf76f15"; }
        }
        public void FillPlayerInfo()
        {
            var teamRepository = new TeamRepository();
            var teams = teamRepository.Retrive();
            var playerRepository = new PlayerRepository();

            foreach(var team in teams)
            {
                var fifaTeamId = getFifaTeamId(team.Id);
                var players = getPlayers(fifaTeamId);


                playerRepository.Insert(players);
            }
        }

        private List<Player> getPlayers(int fifaTeamId)
        {
            var players = new List<Player>();
            return players;
        }

        private int getFifaTeamId(string teamId)
        {
            return 0;
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

    }
}
