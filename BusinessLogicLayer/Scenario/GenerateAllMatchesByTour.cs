﻿using BusinessLogicLayer.Services;
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
        GoalRepository _goalRepository;
        SeasonValueCreator _seasonValueCreator;
        NationalResTabRepository _nationalResTabRepository;
        TeamRatingWinCoeffRepository _teamRatingWinCoeffRepository;


        public GenerateAllMatchesByTour(DateTime gameDate)
        {
            _gameDate = gameDate;
            _scheduleGenerator = new ScheduleMatchGenerator();
            _matchRepository = new MatchRepository();
            _goalRepository = new GoalRepository();
            _seasonValueCreator = new SeasonValueCreator();
            _nationalResTabRepository = new NationalResTabRepository();
            _teamRatingWinCoeffRepository = new TeamRatingWinCoeffRepository();
        }

        public void Generate()
        {
            var matches = _matchRepository.Retrieve(_gameDate);
            if (matches.Count == 0)
            {
                generateSchedule(_gameDate);
                matches = _matchRepository.Retrieve(_gameDate);
                if(matches.Count == 0)
                {
                    throw new Exception("Error");
                }

                insertNewRows(matches);
            }

            var teamsId = getAllTeamsId(matches);
            var allMatches = getMatches(_gameDate);
            generateAllMatches(allMatches);
            _teamRatingWinCoeffRepository.InsertNewTeams(teamsId,_seasonValueCreator.GetSeason(_gameDate.Year));
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

                    allMatches.Add(match);
                }
            }

            _matchRepository.Update(allMatches);
            defineTeamsStats(allMatches);
            _goalRepository.Insert(goalList);
        }
        private void defineTeamsStats(List<Match> matches)
        {
            var teamsWithResult = _nationalResTabRepository.Retrieve(_seasonValueCreator.GetSeason(_gameDate.Year));
            string season = _seasonValueCreator.GetSeason(_gameDate.Year);
            foreach(var match in matches)
            {
                if(teamsWithResult.TryGetValue(match.HomeTeamId,out NationalResultTable homeTeamTabRecord)
                     && teamsWithResult.TryGetValue(match.GuestTeamId, out NationalResultTable guestTeamTabRecord))
                {
                    homeTeamTabRecord.ScoredGoals += match.HomeTeamGoals;
                    guestTeamTabRecord.ScoredGoals += match.GuestTeamGoals;

                    guestTeamTabRecord.MissedGoals += match.HomeTeamGoals;
                    homeTeamTabRecord.MissedGoals += match.GuestTeamGoals;


                    if (homeTeamTabRecord.ScoredGoals > guestTeamTabRecord.ScoredGoals)
                    {
                        homeTeamTabRecord.Wins += 1;
                        guestTeamTabRecord.Loses += 1;
                        homeTeamTabRecord.TotalPoints += 3;
                    }
                    else if (homeTeamTabRecord.ScoredGoals < guestTeamTabRecord.ScoredGoals)
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

        private List<string> getAllTeamsId(Dictionary<int, List<Match>> matches)
        {
            var teamdsIds = new List<string>();
            foreach (var matchesByLeague in matches.Values)
            {
                foreach(var match in matchesByLeague)
                {
                    teamdsIds.Add(match.HomeTeamId);
                    teamdsIds.Add(match.GuestTeamId);
                }
            }
            return teamdsIds.Distinct().ToList();
        }

        private void insertNewRows(Dictionary<int, List<Match>> matches)
        {
            var teamsID = getAllTeamsId(matches);

            foreach (var item in teamsID)
            {
                var tab = createResultTable(item);
                _nationalResTabRepository.Insert(tab);
            }
        }
    }
}