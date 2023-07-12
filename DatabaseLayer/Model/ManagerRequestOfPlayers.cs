using DatabaseLayer.Enums;
using System;

namespace DatabaseLayer.Model
{
    public class ManagerRequestOfPlayers
    {
        public string Id { get; set; }
        public string ManagerId { get; set; }
        public string TeamId { get; set; }
        public ManagerRequestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public FindPlayersCriteria Criteria { get; set; }

        public ManagerRequestOfPlayers()
        {
            Id = new Guid().ToString();
        }
    }
}
