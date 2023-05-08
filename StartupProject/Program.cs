using DatabaseLayer;
using System;
using System.Drawing;
using BusinessLogicLayer;
using BusinessLogicLayer.Services;
using DatabaseLayer.Repositories;
using DatabaseLayer.Services;
using System.IO;
using BusinessLogicLayer.Services.TransferMarketManager;
using System.Linq;

namespace StartupProject
{
    internal class Program
    {
       
        public static void Main(string[] args)
        {

            //var instance = LoadGameManager.GetInstance(Directory.GetCurrentDirectory());

            //SaveInfo saveInfo = new SaveInfo(new PlayerGameData()
            //{
            //    //Date = DateTime.Now.ToString(),
            //    PlayerName = "pl1",
            //    Money = 50,

            //}, "Save5");

            //instance.SaveGame(saveInfo);
            /*var mge = new MatchGeneratingExampleUsing();
            mge.GenerateMatch();*/



            // ------------------------------Test TRANSFER---------------------------------------------------------------

            //var transfer = new TransferMarketManager();
            //var t = new TransferMarketRepository().RetrieveByPlayer("3440c715-31f0-4785-9ef8-7e9032167b94");
            //transfer.SubmitForTransfer("3440c715-31f0-4785-9ef8-7e9032167b94", "CB3A5B757336C346B3A7D946A8A71A36", TransferType.Buy, 750000);
            //var res = transfer.SearchPlayerOnMarket(new DatabaseLayer.Model.TransferMarketSearchParams());
            //transfer.MakeOffer("21874217-8221-4633-8758-323e4dc86218", "B5551778D1672E4E544F32BFFAD52BA6", "015834FD9556AAEC44DE54CDE350235B", 2250000);
            //transfer.MakeOffer("8d8618a4-7ae7-46db-9b92-2303c3e81698", "C36DACD1B8A0577396F96EDE02AB389A", "C3983D7B84A31F178E7EE0D3D9EE1A95", 441000);
            //transfer.MakeOffer("42a42093-8e78-4f80-b833-038b736f1fc2", "A8BD96F988DC2C163940EA1E62354B89", "C3983D7B84A31F178E7EE0D3D9EE1A95", 469000);

            //var ress = transfer.SearchPlayerOnMarket(new DatabaseLayer.Model.TransferMarketSearchParams() { AgeLowerBound = 18});
            //var reso = transfer.GetOfferByTeamBuyer("C3983D7B84A31F178E7EE0D3D9EE1A95");
            //var resov = transfer.GetOfferByTeamSeller("B5551778D1672E4E544F32BFFAD52BA6");
            //var s = new TransferOfferRepository().Retrieve();
            //var id = "";
            //transfer.ChangeOffer(id, 15000000);
            //var resoe = transfer.GetOfferByTeamBuyer("C3983D7B84A31F178E7EE0D3D9EE1A95");
            //var resove = transfer.GetOfferByTeamSeller("B5551778D1672E4E544F32BFFAD52BA6");
            //transfer.AcceptCancelOffer(id, TransferStatus.Done, DateTime.Now, 75000000);
            ////var res1 = transfer.SearchPlayerOnMarket();
            ////var ress1 = transfer.SearchPlayerOnMarket(null, null, 30);
            //var reso1 = transfer.GetOfferByTeamBuyer("C3983D7B84A31F178E7EE0D3D9EE1A95");
            //var resov1 = transfer.GetOfferByTeamSeller("B5551778D1672E4E544F32BFFAD52BA6");
            //Console.WriteLine(res.Select(s => s.DesireAmount).FirstOrDefault());
            //Console.WriteLine(res.Select(s => s.IDPlayer).FirstOrDefault());
            //Console.ReadKey();
        }
    }
}
