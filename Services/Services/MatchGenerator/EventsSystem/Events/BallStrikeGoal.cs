namespace FootBalLife.Services.MatchGenerator
{
    internal class BallStrikeGoalEvent : MatchEventProcess
    {
        public BallStrikeGoalEvent()
        {

        }
        public override void ProcessEvent()
        {
            ScoredPlayer = new Guid(HomeTeam.GetPlayer(Database.PlayerPosition.Attack).PersonID);

            base.ProcessEvent();
        }
    }
}
