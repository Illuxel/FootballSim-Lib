using DatabaseLayer.Model;
using System;

namespace DatabaseLayer
{
    public class Match
    {
        public string Id { get; set; }
        public string MatchDate { get; set; }

        public string HomeTeamId { get; set; }
        public string GuestTeamId { get; set; }

        public int HomeTeamGoals { get; set; }
        public int GuestTeamGoals { get; set; }

        public Team HomeTeamNavigation { get; internal set; }
        public Team GuestTeamNavigation { get; internal set; }

        public int TourNumber { get; set; }
        public int LeagueId { get; set; }
        public League League { get; internal set; }
        public bool IsPlayed { get; set; }
        public DateTime GetMatchDate()
        {
            return DateTime.Parse(MatchDate);
        }
    }
}