using System;

namespace BusinessLogicLayer.Services
{
    internal class BallStrikeGoalEvent : MatchEventProcess
    {
        public BallStrikeGoalEvent()
        {
        }

        //TODO: probability need to save external
        public override void ProcessEvent()
        {
            ScoredPlayer = new Guid(HomeTeam.GetPlayer(42,38,19,0).PersonID);
            var rand = new Random();
            var assistProbability = rand.NextDouble();
            if (assistProbability > 0.4)
            {
                AssistedPlayer = new Guid(HomeTeam.GetPlayer(32, 41, 26, 0).PersonID);
                if (ScoredPlayer == AssistedPlayer)
                {
                    AssistedPlayer = null;
                }
            }
            
            base.ProcessEvent();
        }
    }
}
