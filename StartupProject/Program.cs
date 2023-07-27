using BusinessLogicLayer.Services;
using DatabaseLayer.Repositories;
using System;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            RequestForManagerToScout requestForManagerToScout = new RequestForManagerToScout();

            var id = requestForManagerToScout.FindPlayersAndGetRequestId("1", "015834FD9556AAEC44DE54CDE350235B", DateTime.Now);
            requestForManagerToScout.RequestForScout(id,DateTime.Now.AddDays(7), 100, 80, "RW", 1000000, 0, 70);
            
            var request = new ManagerRequestOfPlayersRepository().RetrieveById(id);
            
            var scoutLookAt = new ScoutLookingForPlayers();
            scoutLookAt.ConfirmRequest(request, "B5551778D1672E4E544F32BFFAD52BA6","ae71f294-86ff-452d-8bdb-f21989180406",DateTime.Now);
        }
    }
}
