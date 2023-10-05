using System.Collections.Generic;

namespace DatabaseLayer
{
    public interface ITeamForMatch
    {
        //TODO: add method for change player
        string Id { get;}
        string Name { get; }
        string BaseColor { get; }
        int AvailablePlayerCount { get; set; }
        StrategyType Strategy { get; set; }

        TacticSchema TacticSchema { get; set; }
        List<Player> AllPlayers { get; set; }
        Dictionary<int, TacticPlayerPosition> MainPlayers { get; set; }

        public List<PlayerInMatch> PlayersInMatch { get; set; }

        List<Player> SparePlayers { get; set; }

        //Created list for spared players
        List<Player> SparedPlayers { get; set; }

        //Inserted method into interface contract
        void SubstitutePlayer(int indexMainPlayer, Player sparePlayer, int currentMinute);
        void ChangeTacticScheme(TacticSchema newTacticSchema);

        Player GetPlayer(PlayerFieldPartPosition playerPostion);
        Player GetPlayer(double attackProb, double midProb, double defProb, double keeperProb);
        double AvgSpeed(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
        double AvgStrike(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
        double AvgDefense(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
        double AvgPhysicalTraining(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
        double AvgTechnique(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
        double AvgPassing(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
    }
}
