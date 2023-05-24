using DatabaseLayer;
using System;

namespace BusinessLogicLayer.Services
{
    public class MatchGenerator
    {
        private MatchResult _matchData;
        private TeamForMatchCreator _teamForMatchCreator;
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

        public MatchGenerator(ITeamForMatch homeTeam, ITeamForMatch guestTeam)
        {
            _matchData = new MatchResult();

            _matchData.MatchID = Guid.NewGuid().ToString();

            _matchData.HomeTeam = homeTeam;
            _matchData.GuestTeam = guestTeam;

            _isMatcFinished = false;
            _isStrategyChanged = false;
        }
        public MatchGenerator(Match match)
        {
            _matchData = new MatchResult();
            _teamForMatchCreator = new TeamForMatchCreator();

            _matchData.MatchID = match.Id;

            _matchData.HomeTeam = _teamForMatchCreator.Create(match.HomeTeamId);
            _matchData.GuestTeam = _teamForMatchCreator.Create(match.GuestTeamId);


            _isMatcFinished = false;
            _isStrategyChanged = false;
        }
        public void StartGenerating()
        {
            if(!_matchData.IsValidMatch())
            {
                return;
            }

            var currentMinute = 0;
            var firstTime = true;
            var strategyEventName = "BallControl";

            while (!_isMatcFinished)
            {
                var currentEvent = MatchEventFactory.CreateStrategyEvent(strategyEventName, HomeTeamStrategy, EventLocation.Center) as MatchEventProcess;

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