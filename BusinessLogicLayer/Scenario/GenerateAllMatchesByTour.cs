using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.Scenario
{
    public class GenerateAllMatchesByTour
    {
        DateTime _gameDate;
        ScheduleMatchGenerator _scheduleGenerator;
        MatchGenerator _matchGenerator;
        MatchRepository _matchRepository;
        LeagueRepository _leagueRepository;
        TeamForMatchCreator _teamForMatchCreator;

        public GenerateAllMatchesByTour(DateTime gameDate)
        {
            _gameDate = gameDate;
            _scheduleGenerator = new ScheduleMatchGenerator();
            _matchRepository = new MatchRepository();
            _leagueRepository = new LeagueRepository();
            _teamForMatchCreator = new TeamForMatchCreator();
        }

        public void GenerateAllMatches()
        {
            generateSchedule(_gameDate);
            var matches = getAllMatches();
            generateAllMatches(matches);
        }

        private void generateSchedule(DateTime gameDate)
        {
            _scheduleGenerator.Generate(gameDate.Year);
        }

        private Dictionary<int,List<Match>> getAllMatches()
        {
            var matchesByLeague = new Dictionary<int, List<Match>>();

            var allLeagues = _leagueRepository.Retrieve();
            
            foreach(var league in allLeagues)
            {
                var id = league.Id;

                var matchesInLeague = _matchRepository.Retrieve(id);
                matchesByLeague.Add(id, matchesInLeague);
            }
            return matchesByLeague;
        }
        
        private void generateAllMatches(Dictionary<int,List<Match>> schedule)
        {
            foreach (var matches in schedule)
            {
                foreach(var match in matches.Value)
                {
                    ITeamForMatch home = _teamForMatchCreator.Create(match.HomeTeamId);
                    ITeamForMatch guest = _teamForMatchCreator.Create(match.GuestTeamId);
                    _matchGenerator = new MatchGenerator(home, guest);
                    _matchGenerator.StartGenerating();
                }
            }
        }
    }
}
