using DatabaseLayer;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public interface ITeamForMatch
    {
        //TODO: add method for change player
        string Id { get;}
        string Name { get; }
        string BaseColor { get; }
        StrategyType Strategy { get; set; }

        TacticSchema TacticSchema { get; set; }
        List<Player> AllPlayers { get; set; }
        Dictionary<int, TacticPlayerPosition> MainPlayers { get; set; }

        List<Player> SparePlayers { get; set; }

        void ChangeTacticScheme(TacticSchema newTacticSchema);

        Player GetPlayer(PlayerFieldPartPosition playerPostion);
        double AvgSpeed(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
        double AvgStrike(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
        double AvgDefense(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
        double AvgPhysicalTraining(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
        double AvgTechnique(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
        double AvgPassing(PlayerFieldPartPosition playerPostion = PlayerFieldPartPosition.All);
    }
}
