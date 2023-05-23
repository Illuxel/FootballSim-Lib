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

        public int HomeTeamGoals = 0;
        public int GuestTeamGoals = 0;
        

        public List<Goal> Goals;

        public Guid? Winner 
        {
            get 
            {
                if(HomeTeam.AllPlayers.Count < 18)
                {
                    return new Guid(GuestTeam.Id);
                }
                else if(GuestTeam.AllPlayers.Count < 18)
                {
                    return new Guid(HomeTeam.Id);
                }

                foreach (var goal in Goals) 
                {
                    if (goal.TeamId == HomeTeam.Id.ToString())
                    {
                        HomeTeamGoals += 1;
                    }
                    else 
                    {
                        GuestTeamGoals += 1;
                    }
                }

                if (HomeTeamGoals > GuestTeamGoals)
                {
                     return new Guid(HomeTeam.Id);
                } 
                if (HomeTeamGoals < GuestTeamGoals)
                {
                     return new Guid(GuestTeam.Id);
                }

                return null;
            }
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
