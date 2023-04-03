using DatabaseLayer;
using System;

namespace BusinessLogicLayer.Services
{
    internal class BallStrikeGoalEvent : MatchEventProcess
    {
        public BallStrikeGoalEvent()
        {

        }
        public override void ProcessEvent()
        {
            ScoredPlayer = new Guid(HomeTeam.GetPlayer(PlayerFieldPartPosition.Attack).PersonID);

            base.ProcessEvent();
        }
    }
}
