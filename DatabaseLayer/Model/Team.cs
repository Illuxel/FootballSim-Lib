using DatabaseLayer.Repositories;
using System.Collections.Generic;

namespace DatabaseLayer
{
    public class Team
    {
        public string Id { get; internal set; }
        public string Name { get; set; }
        public string BaseColor { get; set; }
        public StrategyType Strategy { get; set; }

        //ââåäåòüñÿ âèêëþ÷íî â ªâðî
        public double Budget { get; set; }
        public int IsNationalTeam { get; set; }

        public int LeagueID { get; set; }
        public League League { get; internal set; }

        public string ScoutID { get; set; }
        public string CoachID { get; set; }
        public int ExtId { get; set; }
        public string ExtName { get; set; }
        public string SportsDirectorId { get; set; }
        public TacticSchema TacticSchema { get; set; }
        public int GlobalRating { get; set; }


        private List<Player> _players;
        public List<Player> Players
        {
            get
            {
                if (_players == null)
                {
                    _players = new List<Player>();
                    var playerRepos = new PlayerRepository();
                    var players = playerRepos.Retrieve(Id);
                    _players.AddRange(players);
                }
                return _players;
            }
        }
        /*
        public virtual ICollection<Contract> Contracts { get; internal set; } = new List<Contract>();

        
        public ICollection<Match> MatchHomeTeamNavigations { get; internal set; } = new List<Match>();
        public ICollection<Match> MatchGuestTeamNavigations { get; internal set; } = new List<Match>();
        
        public ICollection<NationalResultTable> NationalResultTables { get; internal set; } = new List<NationalResultTable>();
        */
        // returns first player that have playerPostion
       
    }
}
