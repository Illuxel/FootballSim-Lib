using DatabaseLayer;
using System;

namespace BusinessLogicLayer.Services.MatchGenerator
{
    public class MatchGenerator
    {
        private MatchResult _matchData;
        public MatchResult MatchData { get { return _matchData; } } 

        public StrategyType HomeTeamStrategy
        {
            get
            {
                return MatchData.HomeTeam.Strategy;
            }
            set
            {
                _isStrategyChanged = true;
                MatchData.HomeTeam.Strategy = value;
            }
        }
        public StrategyType GuestTeamStrategy
        {
            get
            {
                return MatchData.GuestTeam.Strategy;
            }
            set
            {
                _isStrategyChanged = true;
                MatchData.GuestTeam.Strategy = value;
            }
        }

        private bool _isStrategyChanged;
        private bool _isMatcFinished;

        public delegate void MatchPausedHandler();
        public event MatchPausedHandler? OnMatchPaused;

        public delegate void GoalHandler(Goal goal);
        public event GoalHandler? OnMatchGoal;

        public delegate void TeamChangedHandler();
        public event TeamChangedHandler? OnMatchTeamChanged;

        public delegate void GameEventHandler(IMatchGameEvent gameEvent);
        public event GameEventHandler? OnMatchEventHappend;

        public MatchGenerator(Team homeTeam, Team guestTeam)
        {
            _matchData = new MatchResult();

            _matchData.MatchID = Guid.NewGuid();

            _matchData.HomeTeam = homeTeam;
            _matchData.GuestTeam = guestTeam;

            _isMatcFinished = false;
            _isStrategyChanged = false;
        }

        public void StartGenerating()
        {
            var currentMinute = 0;
            var firstTime = true;
            var strategyEventName = "BallControl";

            while (!_isMatcFinished)
            {
                MatchEventProcess? currentEvent = MatchEventFactory.CreateStrategyEvent(strategyEventName, HomeTeamStrategy, EventLocation.Center) as MatchEventProcess;

                if (currentEvent == null)
                {
                    throw new Exception("Created strategy event was null");
                }

                currentEvent.HomeTeam = _matchData.HomeTeam;
                currentEvent.GuestTeam = _matchData.GuestTeam;

                while (true)
                {
                    if (currentMinute >= 90)
                    { 
                        _isMatcFinished = true;
                        break;
                    }

                    if (_isStrategyChanged)
                    {
                        _isStrategyChanged = false;
                        break;
                    }

                    if (currentMinute >= 45 && firstTime)
                    {
                        firstTime = false;

                        currentEvent.HomeTeam = _matchData.GuestTeam;
                        currentEvent.GuestTeam = _matchData.HomeTeam;

                        OnMatchPaused?.Invoke();
                    }

                    currentMinute += currentEvent.Duration;
                    currentEvent.MatchMinute = currentMinute;

                    currentEvent.ProcessEvent();

                    _matchData.MatchHistory.Add(currentEvent);

                    if (currentEvent is BallStrikeGoalEvent)
                    {
                        Goal goal = new Goal()
                        {
                            Id = Guid.NewGuid().ToString(),
                            MatchId = _matchData.MatchID.ToString(),
                            PlayerId = currentEvent.ScoredPlayer.Value.ToString(),
                            TeamId = currentEvent.HomeTeam.Id.ToString(),
                            MatchMinute = currentMinute
                        };

                        _matchData.Goals.Add(goal);
                        OnMatchGoal?.Invoke(goal);
                    }

                    OnMatchEventHappend?.Invoke(currentEvent);

                    if (currentEvent.IsBallIntercepted)
                    {
                        var temp = currentEvent.HomeTeam;
                        currentEvent.HomeTeam = currentEvent.GuestTeam;
                        currentEvent.GuestTeam = temp;

                        OnMatchTeamChanged?.Invoke();
                    }

                    var nextEvent = MatchEventFactory.CreateNextEvent(currentEvent);
                    currentEvent = nextEvent as MatchEventProcess;
                }
            }

            finalizeGeneration();
        }

        private void finalizeGeneration()
        {
            foreach (var gameEvent in _matchData.MatchHistory)
            {
                if (gameEvent.AssistedPlayer != null)
                {
                    _matchData.AssistedPlayers.Add(gameEvent.AssistedPlayer.Value);
                }
                if (gameEvent.InjuredPlayer != null)
                {
                    _matchData.InjuredPlayers.Add(gameEvent.InjuredPlayer.Value);
                }
                if (gameEvent.YellowCardPlayer != null)
                {
                    _matchData.YellowCardPlayers.Add(gameEvent.YellowCardPlayer.Value);
                }
                if (gameEvent.RedCardPlayer != null)
                {
                    _matchData.RedCardPlayers.Add(gameEvent.RedCardPlayer.Value);
                }
                if (gameEvent.ScoredPlayer != null)
                {
                    _matchData.ScoredPlayers.Add(gameEvent.ScoredPlayer.Value);
                }
            }
        }
    }
}