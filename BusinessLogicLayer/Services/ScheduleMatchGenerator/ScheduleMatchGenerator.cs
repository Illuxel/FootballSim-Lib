using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class ScheduleMatchGenerator
    {
        private static int _countDaysOfRest = 7;
        private static string _emptyTeamName = "Dummy";


        private SeasonValueCreator _seasonCreator ;
        private LeagueRepository _leagueRepository;
        private TeamRepository _teamRepository;

        public ScheduleMatchGenerator() 
        {
            _seasonCreator = new SeasonValueCreator();
            _leagueRepository = new LeagueRepository();
            _teamRepository = new TeamRepository();
        }
        public void Generate(int year)
        {
            var matches = new List<Match>();
            var leagues = _leagueRepository.Retrieve();
            var firstTourDate = _seasonCreator.GetSeasonStartDate(year);
            foreach (var league in leagues)
            {
                var teams = _teamRepository.Retrieve(league.Id);
                matches.AddRange(generateNationalCalendarMatch(firstTourDate, teams, league.Id));
            }

            var matchRepository = new MatchRepository();
            matchRepository.Insert(matches);
        }

        private List<Match> generateNationalCalendarMatch(DateTime startDate,  List<Team> teams, int leagueId)
        {
            var matches = new List<Match>();

            // Перетасувати команди
            var shuffledTeams = teams.OrderBy(t => Guid.NewGuid()).ToList();

            var currentDate = startDate;
            // Додати маникен команди, якщо не парна кількість команд
            if (shuffledTeams.Count % 2 != 0)
            {
                shuffledTeams.Add(new Team { Name = _emptyTeamName });
            }

            int totalTours = shuffledTeams.Count - 1;
            int numMatchesPerRound = shuffledTeams.Count / 2;

            for (int currentTour = 1; currentTour <= totalTours; currentTour++)
            {
                for (int matchNumber = 0; matchNumber < numMatchesPerRound; matchNumber++)
                {
                    int homeTeamIndex = (currentTour + matchNumber) % (shuffledTeams.Count - 1);
                    int awayTeamIndex = (shuffledTeams.Count - 1 - matchNumber + currentTour) % (shuffledTeams.Count - 1);

                    if (matchNumber == 0)
                    {
                        awayTeamIndex = shuffledTeams.Count - 1;
                    }

                    var homeTeam = shuffledTeams[homeTeamIndex];
                    var guestTeam = shuffledTeams[awayTeamIndex];
                    if (homeTeam.Name != _emptyTeamName && guestTeam.Name != _emptyTeamName)
                    {
                        matches.Add(new Match
                        {
                            Id = Guid.NewGuid().ToString(),
                            HomeTeamId = homeTeam.Id,
                            GuestTeamId = guestTeam.Id,
                            TourNumber = currentTour,
                            LeagueId = leagueId,
                            MatchDate = currentDate.ToString("yyyy-MM-dd"),
                            Season = _seasonCreator.GetSeason(currentDate.Year)
                        });

                        //матч-відповідь
                        matches.Add(new Match
                        {
                            Id = Guid.NewGuid().ToString(),
                            HomeTeamId = guestTeam.Id,
                            GuestTeamId = homeTeam.Id,
                            TourNumber = (teams.Count - 1) + currentTour,
                            LeagueId = leagueId,
                            MatchDate = currentDate.AddDays((_countDaysOfRest * (teams.Count - 1))).ToString("yyyy-MM-dd"),
                            Season = _seasonCreator.GetSeason(currentDate.Year)
                        });
                    }
                }
                currentDate = currentDate.AddDays(_countDaysOfRest);
            }

            return matches;
        }
    }
}
