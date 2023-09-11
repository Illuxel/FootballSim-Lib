using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Scenario
{
    internal class GenerateAllMatchesByTour
    {
        DateTime _gameDate;
        private string _ownerTeamId;
        ScheduleMatchGenerator _scheduleGenerator;
        MatchGenerator _matchGenerator;
        MatchRepository _matchRepository;
        GoalRepository _goalRepository;
        SeasonValueCreator _seasonValueCreator;
        NationalResTabRepository _nationalResTabRepository;
        TeamRatingWinCoeffRepository _teamRatingWinCoeffRepository;
        TeamRepository _teamRepository;


        public GenerateAllMatchesByTour(DateTime gameDate, string ownerTeamId)
        {
            _gameDate = gameDate;
            _ownerTeamId = ownerTeamId;
            _scheduleGenerator = new ScheduleMatchGenerator();
            _matchRepository = new MatchRepository();
            _goalRepository = new GoalRepository();
            _seasonValueCreator = new SeasonValueCreator();
            _nationalResTabRepository = new NationalResTabRepository();
            _teamRatingWinCoeffRepository = new TeamRatingWinCoeffRepository();
            _teamRepository = new TeamRepository();
        }

        public void Generate()
        {
            var nationalResultTabs = _nationalResTabRepository.Retrieve(_seasonValueCreator.GetSeason(_gameDate));

            if (nationalResultTabs.Count == 0)
            {
                generateSchedule(_gameDate);

                var firstTourMatches = _matchRepository.Retrieve(1);
                insertNewRowsToDatabase(firstTourMatches);
            }
            var allMatchesByTour = getMatches(_gameDate);
            if(allMatchesByTour.Count != 0)
            {
                generateAllMatches(allMatchesByTour);
            }
        }

        private void generateSchedule(DateTime gameDate)
        {
            _scheduleGenerator.Generate(gameDate.Year);
        }

        private Dictionary<int,List<Match>> getMatches(DateTime tourDate)
        {
            var matchesByLeague = _matchRepository.Retrieve(tourDate);

            return matchesByLeague;
        }
        
        private void generateAllMatches(Dictionary<int,List<Match>> schedule)
        {
            List<Match> allMatches = new List<Match>();
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
                    match.IsPlayed = true;
                    allMatches.Add(match);
                }
            }
            
            _matchRepository.Update(allMatches);
            defineTeamsStats(allMatches);
            _goalRepository.Insert(goalList);
        }
        private void defineTeamsStats(List<Match> matches)
        {
            var season = _seasonValueCreator.GetSeason(_gameDate);
            var teamsWithResult = _nationalResTabRepository.Retrieve(season);
            if(teamsWithResult.Count == 0)
            {
                season = _seasonValueCreator.GetSeason(_gameDate.Year - 1);
                teamsWithResult = _nationalResTabRepository.Retrieve(season);
            }
            foreach(var match in matches)
            {
                if(teamsWithResult.TryGetValue(match.HomeTeamId,out NationalResultTable homeTeamTabRecord) && teamsWithResult.TryGetValue(match.GuestTeamId, out NationalResultTable guestTeamTabRecord))
                {
                    homeTeamTabRecord.ScoredGoals += match.HomeTeamGoals;
                    guestTeamTabRecord.ScoredGoals += match.GuestTeamGoals;

                    guestTeamTabRecord.MissedGoals += match.HomeTeamGoals;
                    homeTeamTabRecord.MissedGoals += match.GuestTeamGoals;


                    if (match.HomeTeamGoals > match.GuestTeamGoals)
                    {
                        homeTeamTabRecord.Wins += 1;
                        guestTeamTabRecord.Loses += 1;
                        homeTeamTabRecord.TotalPoints += 3;
                    }
                    else if (match.HomeTeamGoals < match.GuestTeamGoals)
                    {
                        homeTeamTabRecord.Loses += 1;
                        guestTeamTabRecord.Wins += 1;
                        guestTeamTabRecord.TotalPoints += 3;
                    }
                    else
                    {
                        homeTeamTabRecord.Draws += 1;
                        guestTeamTabRecord.Draws += 1;
                        homeTeamTabRecord.TotalPoints += 1;
                        guestTeamTabRecord.TotalPoints += 1;
                    }
                }
            }
            _nationalResTabRepository.Update(teamsWithResult.Values.ToList(), season);
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
                MissedGoals = 0,
                TotalPoints = 0,
            };
            return resultTable;
        }

        private List<string> getAllTeamsId(List<Match> matches)
        {
            var teamdsIds = new List<string>();
            foreach (var m in matches)
            {
                teamdsIds.Add(m.HomeTeamId);
                teamdsIds.Add(m.GuestTeamId);
            }
            return teamdsIds.Distinct().ToList();
        }

        private void insertNewRowsToDatabase(List<Match> matches)
        {
            var teamsID = getAllTeamsId(matches);

            foreach (var item in teamsID)
            {
                var tab = createResultTable(item);
                _nationalResTabRepository.Insert(tab);
            }

            _teamRatingWinCoeffRepository.InsertNewTeams(teamsID, _seasonValueCreator.GetSeason(_gameDate));
        }
    }
}
