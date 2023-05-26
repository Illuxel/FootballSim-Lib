using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Scenario
{
    public class GenerateAllMatchesByTour
    {
        DateTime _gameDate;
        ScheduleMatchGenerator _scheduleGenerator;
        MatchGenerator _matchGenerator;
        MatchRepository _matchRepository;
        LeagueRepository _leagueRepository;
        GoalRepository _goalRepository;
        SeasonValueCreator _seasonValueCreator;
        NationalResTabRepository _nationalResTabRepository;
        RatingActualizer _ratingActualizer;
        TeamRatingWinCoeffRepository _teamRatingWinCoeffRepository;


        public GenerateAllMatchesByTour(DateTime gameDate)
        {
            _gameDate = gameDate;
            _scheduleGenerator = new ScheduleMatchGenerator();
            _matchRepository = new MatchRepository();
            _leagueRepository = new LeagueRepository();
            _goalRepository = new GoalRepository();
            _seasonValueCreator = new SeasonValueCreator();
            _nationalResTabRepository = new NationalResTabRepository();
            _ratingActualizer = new RatingActualizer();
            _teamRatingWinCoeffRepository = new TeamRatingWinCoeffRepository();
        }

        public void Generate()
        {
            var matches = _matchRepository.Retrieve(_gameDate);
            var teamsId = getAllTeamsId(matches);
            if (matches.Count == 0)
            {
                generateSchedule(_gameDate);
                matches = _matchRepository.Retrieve(_gameDate);
                if(matches.Count == 0)
                {
                    throw new Exception("Error");
                }

                teamsId = getAllTeamsId(matches);
                insertNewRows(matches);
            }

            var allMatches = getAllMatches();
            generateAllMatches(allMatches);
            _teamRatingWinCoeffRepository.InsertNewTeams(teamsId,_seasonValueCreator.GetSeason(_gameDate.Year));
            _ratingActualizer.Actualize(_gameDate);

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
            List<Goal> goalList = new List<Goal>();
            foreach (var matches in schedule)
            {
                foreach(var match in matches.Value)
                {            
                    _matchGenerator = new MatchGenerator(match);
                    _matchGenerator.StartGenerating();
                    
                    var result = _matchGenerator.MatchData;
                    goalList.AddRange(result.Goals);

                    match.HomeTeamGoals = result.HomeTeamGoals;
                    match.GuestTeamGoals = result.GuestTeamGoals;

                    _matchRepository.Update(match);
                    defineTeamsStats(match);
                }
            }

            _goalRepository.Insert(goalList);
        }

        private void defineTeamsStats(Match match)
        {
            string season = _seasonValueCreator.GetSeason(_gameDate.Year);
            var homeTeamTabRecord = _nationalResTabRepository.Retrieve(match.HomeTeamId, season).FirstOrDefault();
            var guestTeamTabRecord = _nationalResTabRepository.Retrieve(match.GuestTeamId, season).FirstOrDefault();

            homeTeamTabRecord.ScoredGoals += match.HomeTeamGoals;
            homeTeamTabRecord.MissedGoals += match.GuestTeamGoals;


            guestTeamTabRecord.ScoredGoals += match.GuestTeamGoals;
            guestTeamTabRecord.MissedGoals += match.HomeTeamGoals;

            if(homeTeamTabRecord.ScoredGoals > guestTeamTabRecord.ScoredGoals)
            {
                homeTeamTabRecord.Wins += 1;
                guestTeamTabRecord.Loses += 1;
            }
            else if(homeTeamTabRecord.ScoredGoals < guestTeamTabRecord.ScoredGoals)
            {
                homeTeamTabRecord.Loses += 1;
                guestTeamTabRecord.Wins += 1;
            }
            else
            {
                homeTeamTabRecord.Draws += 1;
                guestTeamTabRecord.Draws += 1;
            }

            _nationalResTabRepository.Update(homeTeamTabRecord,guestTeamTabRecord,season);
        }

        private NationalResultTable createResultTable(string teamId)
        {
            NationalResultTable resultTable = new NationalResultTable
            {
                TeamID = teamId,
                Season = _seasonValueCreator.GetSeason(_gameDate.Year),
                Wins = 0,
                Draws = 0,
                Loses = 0,
                ScoredGoals = 0,
                MissedGoals = 0
            };
            return resultTable;
        }

        private List<string> getAllTeamsId(List<Match> matches)
        {
            var home = matches.Select(x => x.HomeTeamId).ToList();
            var guest = matches.Select(x => x.GuestTeamId).ToList();
            var teams = home;
            teams.AddRange(guest);
            return teams.Distinct().ToList();
        }


        private void insertNewRows(List<Match> matches)
        {
            var teamsID = getAllTeamsId(matches);

            foreach (var item in teamsID)
            {
                var tab = createResultTable(item);
                _nationalResTabRepository.Insert(tab);
            }
        }
        //Написати реквест до національної таблиці з можливістю оновлення господарів та гостів матчу
        //Написати метод для врахування перемоги чи поразки = обновити таблицю NationalRes
    }
}
