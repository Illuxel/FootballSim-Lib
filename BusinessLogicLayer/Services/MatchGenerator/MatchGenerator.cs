﻿using DatabaseLayer;
using DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class MatchGenerator
    {
        private MatchResult _matchData;
        private TeamForMatchCreator _teamForMatchCreator;
        private PlayerInMatchRepository _playerInMatchRepository;
        private DateTime _matchDate;
        private Dictionary<string,Player> _playedPlayers;
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
        private PlayerInjuryFinder _playerInjuryFinder;
        private PlayerRepository _playerRepository;
        private MatchRepository _matchRepository;

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

        public delegate void MatchFinishedHandler(MatchResult result);
        public event MatchFinishedHandler? OnMatchFinished;

        public MatchGenerator(ITeamForMatch homeTeam, ITeamForMatch guestTeam)
        {
            _matchData = new MatchResult();
            _playerInMatchRepository = new PlayerInMatchRepository();
            _playerInjuryFinder = new PlayerInjuryFinder();
            _playerRepository = new PlayerRepository();
            _matchRepository = new MatchRepository();

            _matchData.MatchID = Guid.NewGuid().ToString();

            _matchData.HomeTeam = homeTeam;
            _matchData.GuestTeam = guestTeam;

            _isMatcFinished = false;
            _isStrategyChanged = false;

            _playedPlayers = new Dictionary<string,Player>();
        }
        public MatchGenerator(Match match)
        {
            _matchData = new MatchResult();
            _teamForMatchCreator = new TeamForMatchCreator();
            _playerInMatchRepository = new PlayerInMatchRepository();
            _playerInjuryFinder = new PlayerInjuryFinder();
            _playerRepository = new PlayerRepository();
            _matchRepository = new MatchRepository();


            _matchData.MatchID = match.Id;

            _matchData.HomeTeam = _teamForMatchCreator.Create(match.HomeTeamId);
            _matchData.GuestTeam = _teamForMatchCreator.Create(match.GuestTeamId);


            _isMatcFinished = false;
            _isStrategyChanged = false;
            
            _playedPlayers = new Dictionary<string,Player>();
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
            _matchDate = DateTime.Parse(_matchRepository.RetrieveMatchById(_matchData.MatchID).MatchDate);
            //

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
                    var nextEvent = MatchEventFactory.CreateNextEvent(currentEvent);
                    currentEvent = nextEvent as MatchEventProcess;

                    if (_isStrategyChanged)
                    {
                        _isStrategyChanged = false;
                        break;
                    }

                    if(currentMinute % 3 == 0)
                    {
                        enduranceDecrease();
                    }

                    currentEvent.MatchMinute = currentMinute;

                    currentEvent.ProcessEvent();

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
                        if(currentEvent.AssistedPlayer.HasValue)
                        {
                            goal.AssistPlayerId = currentEvent.AssistedPlayer.Value.ToString();
                        }
                        //TODO: remove after find better solution
                        //check goal from free kick or penalty
                        var goalsWithoutAssistEvents = new List<string>() { "Penalty", "FreeKick" };
                        if(_matchData.MatchHistory.
                            Where(item => item.MatchMinute == currentMinute - 3 
                            && goalsWithoutAssistEvents.Contains(item.EventCode)).Count() > 0)
                        {
                            goal.AssistPlayerId = string.Empty;
                            currentEvent.AssistedPlayer = Guid.Empty;
                        }

                        _matchData.Goals.Add(goal);
                        OnMatchGoal?.Invoke(goal);
                    }

                    OnMatchEventHappend?.Invoke(currentEvent);

                    _matchData.MatchHistory.Add(currentEvent);
                    saveEvent(currentEvent);
                    currentMinute += currentEvent.Duration;
                    if(currentEvent.InjuredPlayer.HasValue || currentEvent.RedCardPlayer.HasValue)
                    {
                        OnMatchPaused?.Invoke();
                    }
                    if (currentMinute >= 45 && firstTime)
                    {
                        firstTime = false;

                        currentEvent.HomeTeam = _matchData.GuestTeam;
                        currentEvent.GuestTeam = _matchData.HomeTeam;

                        OnMatchPaused?.Invoke();
                    }

                    if (currentEvent.IsBallIntercepted)
                    {
                        var temp = currentEvent.HomeTeam;
                        currentEvent.HomeTeam = currentEvent.GuestTeam;
                        currentEvent.GuestTeam = temp;

                        OnMatchTeamChanged?.Invoke();
                    }
                    if (currentMinute >= 90 && !(currentEvent is BallStrikeEvent))
                    {
                        _isMatcFinished = true;
                        OnMatchFinished?.Invoke(_matchData);
                        break;
                    }
                }
            }
            foreach (var player in MatchData.HomeTeam.PlayersInMatch)
            {
                player.MatchId = MatchData.MatchID;
            }
            foreach (var player in MatchData.GuestTeam.PlayersInMatch)
            {
                player.MatchId = MatchData.MatchID;
            }
            
            var playersInMatch = MatchData.HomeTeam.PlayersInMatch.Concat(MatchData.GuestTeam.PlayersInMatch).ToList();
            _playerInMatchRepository.Insert(playersInMatch);

            var players = _playedPlayers.Values.ToList();
            updateEnduranceValuesForPlayers(players);
            
            //finalizeGeneration();
        }

        private void saveEvent(IMatchGameEvent matchGameEvent)
        {
            if (matchGameEvent != null)
            {
                if (matchGameEvent.AssistedPlayer != null)
                {
                    _matchData.AssistedPlayers.Add(matchGameEvent.AssistedPlayer.Value);
                }
                if (matchGameEvent.InjuredPlayer != null)
                {
                    _matchData.InjuredPlayers.Add(matchGameEvent.InjuredPlayer.Value);
                }
                if (matchGameEvent.YellowCardPlayer != null)
                {
                    _matchData.YellowCardPlayers.Add(matchGameEvent.YellowCardPlayer.Value);
                    int yellowCardsByCurrentPlayer = _matchData.YellowCardPlayers.
                        Where(item => item == matchGameEvent.YellowCardPlayer.Value).Count();
                    if (yellowCardsByCurrentPlayer == 2)
                    {
                        matchGameEvent.RedCardPlayer = matchGameEvent.YellowCardPlayer.Value;
                    }
                }
                if (matchGameEvent.RedCardPlayer != null)
                {
                    _matchData.RedCardPlayers.Add(matchGameEvent.RedCardPlayer.Value);
                    removePlayer(matchGameEvent.RedCardPlayer.Value);
                }
                if (matchGameEvent.ScoredPlayer != null)
                {
                    _matchData.ScoredPlayers.Add(matchGameEvent.ScoredPlayer.Value);
                }
                
            }
        }

        private void removePlayer(Guid playerId)
        {
            foreach (var player in _matchData.GuestTeam.MainPlayers)
            {
                if (player.Value.CurrentPlayer != null && new Guid(player.Value.CurrentPlayer.PersonID) == playerId)
                {
                    player.Value.CurrentPlayer = null;
                    _matchData.GuestTeam.AvailablePlayerCount -= 1;
                    break;
                }
            }
            foreach (var player in _matchData.HomeTeam.MainPlayers)
            {
                if (player.Value.CurrentPlayer != null && new Guid(player.Value.CurrentPlayer.PersonID) == playerId)
                {
                    player.Value.CurrentPlayer = null;
                    _matchData.HomeTeam.AvailablePlayerCount -= 1;
                    break;
                }
            }
        }

        private void enduranceDecrease()
        {
            foreach (var player in MatchData.HomeTeam.MainPlayers.Values)
            {
                if (player.CurrentPlayer == null)
                {
                    break;
                }
                if (!_playerInjuryFinder.IsAlreadyInjuried(player.CurrentPlayer))
                {
                    updatePlayerEndurance(player.CurrentPlayer);
                    checkAndSetPlayerInjury(player.CurrentPlayer);
                }
            }

            foreach (var player in MatchData.GuestTeam.MainPlayers.Values)
            {
                if (player.CurrentPlayer == null)
                {
                    break;
                }
                if (!_playerInjuryFinder.IsAlreadyInjuried(player.CurrentPlayer))
                {
                    updatePlayerEndurance(player.CurrentPlayer);
                    checkAndSetPlayerInjury(player.CurrentPlayer);
                }
            }
        }

        private void updatePlayerEndurance(Player currentPlayer)
        {
            if (_playedPlayers.TryGetValue(currentPlayer.ContractID, out var playerObject))
            {
                playerObject.Endurance -= (int)Math.Round(currentPlayer.Endurance * 0.02);
            }
            else
            {
                currentPlayer.Endurance -= (int)Math.Round(currentPlayer.Endurance * 0.02);
                _playedPlayers.Add(currentPlayer.ContractID, currentPlayer);
            }
        }

        private void checkAndSetPlayerInjury(Player currentPlayer)
        {
            if (_playerInjuryFinder.IsInjuried(currentPlayer))
            {
                _playerInjuryFinder.SetInjury(currentPlayer, _matchDate);
            }
        }

        private void updateEnduranceValuesForPlayers(List<Player> players)
        {
            _playerRepository.UpdateEndurance(players);
        }

        private void finalizeGeneration()
        {
            foreach (var gameEvent in _matchData.MatchHistory)
            {
                saveEvent(gameEvent);
            }
        }
    }
}