using DatabaseLayer;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    public class MatchResult
    {
        public string MatchID;

        public ITeamForMatch HomeTeam { get; internal set; }
        public ITeamForMatch GuestTeam { get; internal set; }

        private int? _homeTeamGoals;
        public int HomeTeamGoals
        {
            get
            {
                if(_homeTeamGoals == null)
                {
                    _homeTeamGoals = 0;
                    foreach (var goal in Goals)
                    {
                        if (goal.TeamId == HomeTeam.Id.ToString())
                        {
                            _homeTeamGoals += 1;
                        }
                    }
                }
                return _homeTeamGoals ?? 0;
            }
        }

        private int? _guestTeamGoals;
        public int GuestTeamGoals
        {
            get
            {
                if (_guestTeamGoals == null)
                {
                    _guestTeamGoals = 0;
                    foreach (var goal in Goals)
                    {
                        if (goal.TeamId == GuestTeam.Id.ToString())
                        {
                            _guestTeamGoals += 1;
                        }
                    }
                }
                return _guestTeamGoals ?? 0;
            }
        }
        

        public List<Goal> Goals;

        public string? Winner 
        {
            get 
            {
                if (HomeTeamGoals > GuestTeamGoals || GuestTeam.AllPlayers.Count < 18)
                {
                     return HomeTeam.Id;
                } 
                if (HomeTeamGoals < GuestTeamGoals || HomeTeam.AllPlayers.Count < 18)
                {
                     return GuestTeam.Id;
                }

                return null;
            }
        }

        public bool IsValidMatch()
        {
            if (GuestTeam.AllPlayers.Count < 18 || HomeTeam.AllPlayers.Count < 18)
            {
                return false;
            }
            return true;
        }

        public List<IMatchGameEvent> MatchHistory { get; set; }

        public List<Guid> AssistedPlayers { get; set; }
        public List<Guid> InjuredPlayers { get; set; }
        public List<Guid> YellowCardPlayers { get; set; }
        public List<Guid> RedCardPlayers { get; set; }
        public List<Guid> ScoredPlayers { get; set; }

        public MatchResult() 
        {
            AssistedPlayers = new List<Guid>();
            InjuredPlayers = new List<Guid>(); 
            YellowCardPlayers = new List<Guid>(); 
            RedCardPlayers = new List<Guid>(); 
            ScoredPlayers = new List<Guid>();

            Goals = new List<Goal>();
            MatchHistory = new List<IMatchGameEvent>();
        }
    }
}
