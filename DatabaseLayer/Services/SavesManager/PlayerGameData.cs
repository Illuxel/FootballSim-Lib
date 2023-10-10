using System;
using DatabaseLayer.Enums;

namespace DatabaseLayer.Services
{
    public class PlayerGameData
    {
        public string PlayerName { get; set; }
        public string PlayerSurname { get; set; }

        public UserRole Role { get; set; }
        public TrainingMode SelectedTrainingMode { get; set; }
        public ScoutSkillLevel CurrentLevel { get; set; }
        public int CountAvailableScoutRequests { get; set; }

        public string GameDate { get; set; }
        public double Money { get; set; }

        public string PersonId { get; set; }
        public string ClubId { get; set; }

        public PlayerGameData()
        {
            PersonId = Guid.NewGuid().ToString();
        }
    }
}
