using DatabaseLayer;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services.MatchGenerator
{
    public class MatchResult
    {
        public Guid MatchID;

        public Team HomeTeam { get; internal set; }
        public Team GuestTeam { get; internal set; }

        public List<Goal> Goals;

        public Guid? Winner 
        {
            get 
            {
                int homeTeamGoals = 0, guestTeamGoals = 0;

                foreach (var goal in Goals) 
                {
                    if (goal.TeamId == HomeTeam.Id.ToString())
                    {
                        homeTeamGoals += 1;
                    }
                    else 
                    {
                        guestTeamGoals += 1;
                    }
                }

                if (homeTeamGoals > guestTeamGoals)
                {
                     return new Guid(HomeTeam.Id);
                } 
                if (homeTeamGoals < guestTeamGoals)
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
