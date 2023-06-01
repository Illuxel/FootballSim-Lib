using BusinessLogicLayer.Services;
using DatabaseLayer;
using DatabaseLayer.Repositories;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayerTests.Services
{
    public class TeamForMatchCreatorTest
    {
        List<Player> team1 = new List<Player> {
                new PlayerRepository().RetrieveOne("f25634cc-5a22-42dd-a5aa-793790aab438"),//gk

                new PlayerRepository().RetrieveOne("8774fcd9-96b1-4855-be1c-147961d41453"),//rb

                new PlayerRepository().RetrieveOne("72865476-bfe7-40c7-ab12-37cafd9efbed"),//cb
                new PlayerRepository().RetrieveOne("9bd4320f-421d-434d-9712-4c1ea5f70d6f"),//cb

                new PlayerRepository().RetrieveOne("07bb5301-0423-4aaa-bfdb-d88276a9c7d4"),//lb

                new PlayerRepository().RetrieveOne("baae5bc0-16c5-42a4-b2da-193fcc48377a"),//cm
                new PlayerRepository().RetrieveOne("f77c2f1a-3259-4680-9fe4-d7f1afc9ee11"),//cm
                new PlayerRepository().RetrieveOne("e8895290-04b4-4ce6-9129-76d6b768b679"),//cm

                new PlayerRepository().RetrieveOne("d2dde511-c925-448c-ba57-7ed253a569f2"),// st
                new PlayerRepository().RetrieveOne("838b7e3a-fc11-4583-9b5f-6d8e4565edc3"),// st
                new PlayerRepository().RetrieveOne("b32c46eb-5776-46ef-94db-39c29c4dd72d"),// st
            };
        [Test]
        public void CreateTeamForMatch()
        {
            var teamForMatch = new TeamForMatchCreator();
            var result = teamForMatch.Create("3809238CBB4AA8274B555A7B0750FCE5");

            Assert.AreEqual(team1, result.MainPlayers.Values.Select(s => s.CurrentPlayer).ToList());
        }
    }
}
