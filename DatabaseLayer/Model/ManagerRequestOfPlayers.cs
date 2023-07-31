using DatabaseLayer.Enums;
using System;

namespace DatabaseLayer.Model
{
    public class ManagerRequestOfPlayers
    {
        public string Id { get; set; }
        public string ManagerId { get; set; }
        public string TeamId { get; set; }
        public int BudgetLimit { get; set; }
        public ManagerRequestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string PlayerId { get; set; }

        private string? criteriaJSON;

        public FindPlayersCriteria Criteria
        {
            get
            {
                return new FindPlayersCriteria().Deserialize(criteriaJSON);
            }
            set
            {
                criteriaJSON = new FindPlayersCriteria().ConvertToJSON(value);
            }
        }

        internal string? CriteriaJSON
        {
            get 
            { 
                return criteriaJSON; 
            }
            set 
            { 
                criteriaJSON = value; 
            }
        }

        public ManagerRequestOfPlayers()
        {
            Id = Guid.NewGuid().ToString();
            CriteriaJSON = null;
        }
    }
}
