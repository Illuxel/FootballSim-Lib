namespace FootBalLife.Services.MatchGenerator.Events
{
    internal class BallInterceptionEvent : MatchEventProcess
    {
        public BallInterceptionEvent()
        {
            
        }
        public override void OnProcessEvent()
        {
            var tempHomeTeam = HomeTeam;
            HomeTeam = GuestTeam;
            GuestTeam = tempHomeTeam;

            base.OnProcessEvent();
        }
    }
}
