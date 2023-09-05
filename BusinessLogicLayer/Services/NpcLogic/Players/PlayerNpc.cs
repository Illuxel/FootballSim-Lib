using System;
using DatabaseLayer;
using DatabaseLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public class PlayerNpc
    {
        private string _personID { get; set; }

        public PlayerNpc(string personID) 
        {
            _personID = personID;
        }

        public bool AcceptOffer(JobRequest jobRequest)
        {
            var playerRepo = new PlayerRepository();
            var player = playerRepo.RetrieveOne(_personID);
            var playerAge = -player.Person.Birthday.Subtract(DateTime.Today).Days / 365;
            var playerPrice = new PlayerPriceCalculator().GetPlayerSalary(player);

            var rand = new Random();
            double randValue;

            if (playerAge < 30)
            {
                randValue = 9 + rand.Next(1,2); 
            }
            else if (30 <= playerAge || playerAge <= 34)
            {
                randValue = rand.Next(-22, 2);
            }         
            else 
            {
                randValue = rand.Next(-30, 0);
            }

            var modifierSalary = playerPrice * randValue / 100;
            var actualSalary = playerPrice + modifierSalary;

            return actualSalary >= jobRequest.Salary;
        }
    }
}